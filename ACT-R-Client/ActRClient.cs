using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nyctico.Actr.Client.AddCommandRequests;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.EvaluationRequests;
using Nyctico.Actr.Client.MonitorRequests;

namespace Nyctico.Actr.Client
{
    /// <summary>
    ///     Client for interaction with an ACT-R Dispatcher.
    /// </summary>
    public class ActRClient : IDisposable
    {
        private readonly ConcurrentDictionary<string, AbstractAddCommandRequest> _abstractCommands =
            new ConcurrentDictionary<string, AbstractAddCommandRequest>();

        private readonly string _host;
        private readonly int _port;

        private readonly BlockingCollection<Message> _messageQueueIncoming = new BlockingCollection<Message>();
        private readonly BlockingCollection<object> _messageQueueOutgoing = new BlockingCollection<object>();
        private readonly ConcurrentDictionary<string, MonitorRequest> _monitors =
            new ConcurrentDictionary<string, MonitorRequest>();
        private readonly ConcurrentDictionary<int, Result> _resultQueue = new ConcurrentDictionary<int, Result>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Task _evaluateTask;
        private Task _incomingTask;
        private Task _outgoingTask;
        private int _idCount = 1;
        private bool _running = true;
        private TcpClient _socket;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;

        /// <summary>
        ///     Creates a new ACT-R Client
        /// </summary>
        /// <param name="host">Host of the ACT-R Environment</param>
        /// <param name="port">Port of the ACT-R Environment</param>
        public ActRClient(string host, int port)
        {
            _host = host;
            _port = port;

            StartTcpConnection();
            StartReceivingThread();
            StartEvaluatingThread();
            StartOutgoingThread();
        }

        /// <summary>
        ///     Disconnect client
        /// </summary>
        public void Dispose()
        {
            _running = false;
            _cancellationTokenSource.Cancel(false);
            _streamWriter.Close();
            _streamReader.Close();
            _socket.Close();
            _evaluateTask.Wait();
            _outgoingTask.Wait();
            _incomingTask.Wait();
        }

        /// <summary>
        ///     Adds a hook to the dispachter (aka add-command)
        /// </summary>
        /// <param name="addCommandRequest">Hook request</param>
        public void AddDispatcherCommand(AbstractAddCommandRequest addCommandRequest)
        {
            var result = SendMessage("check", addCommandRequest.PublishedNameAsList);
            if (result.AllRetruns != null && result.ReturnObject != null) return;
            SendMessage("add", addCommandRequest.ToParameterList());
            _abstractCommands.TryAdd(addCommandRequest.PrivateName, addCommandRequest);
        }

        /// <summary>
        ///     Removes a hook from the dispatcher (aka remove-command)
        /// </summary>
        /// <param name="addCommandRequest">Hook request</param>
        public void RemoveDispatcherCommand(AbstractAddCommandRequest addCommandRequest)
        {
            var result = SendMessage("check", addCommandRequest.PublishedNameAsList);
            if (result.AllRetruns == null || result.ReturnObject == null) return;
            SendMessage("remove", addCommandRequest.PublishedNameAsList);
            _abstractCommands.TryRemove(addCommandRequest.PrivateName, out addCommandRequest);
        }

        /// <summary>
        ///     Removes a hook from the dispatcher (aka remove-command)
        /// </summary>
        /// <param name="privateName">Private name of the hook</param>
        /// <exception cref="KeyNotFoundException">Thrown when the private name is not found</exception>
        public void RemoveDispatcherCommand(string privateName)
        {
            AbstractAddCommandRequest abstractAddCommandRequest;
            if (!_abstractCommands.TryRemove(privateName, out abstractAddCommandRequest))
                throw new KeyNotFoundException("DispatcherHook not found!");
            RemoveDispatcherCommand(abstractAddCommandRequest);
        }

        /// <summary>
        ///     Adds a monitor to the dispatcher (aka monitor)
        /// </summary>
        /// <param name="monitorRequest">Monitor request</param>
        public void AddDispatcherMonitor(MonitorRequest monitorRequest)
        {
            SendMessage("monitor", monitorRequest.ToParameterList());
            _monitors.TryAdd(monitorRequest.CommandToMonitor + monitorRequest.CommandToCall, monitorRequest);
        }

        /// <summary>
        ///     Removes monitor from dispatcher (aka remove-monitor)
        /// </summary>
        /// <param name="monitorRequest">Monitor request</param>
        public void RemoveDispatcherMonitor(MonitorRequest monitorRequest)
        {
            SendMessage("remove-monitor", monitorRequest.ToParameterList());
            _monitors.TryRemove(monitorRequest.CommandToMonitor + monitorRequest.CommandToCall,
                out monitorRequest);
        }

        /// <summary>
        ///     Removes monitor from dispatcher (aka remove-monitor)
        /// </summary>
        /// <param name="commanToMonitor">Command to monitor</param>
        /// <param name="commandToCall">Command to call</param>
        /// <exception cref="KeyNotFoundException">Thrown when the monitor is not found</exception>
        public void RemoveDispatcherMonitor(string commanToMonitor, string commandToCall)
        {
            MonitorRequest monitorRequest;
            if (!_monitors.TryRemove(commanToMonitor + commandToCall, out monitorRequest))
                throw new KeyNotFoundException("Monitor not found!");
            RemoveDispatcherMonitor(monitorRequest);
        }

        /// <summary>
        ///     Add hooks and monitor to all traces.
        /// </summary>
        /// <param name="traceAction">Function to handle the trace output</param>
        public void StartTraceMonitoring(Action<object[]> traceAction)
        {
            var commandName = ToString() + "_TraceMonitor";
            AbstractAddCommandRequest addCommandRequest =
                new AddCommandRequest(traceAction, commandName, "printTrace", "Trace monitoring");
            AddDispatcherCommand(addCommandRequest);

            var modelDispatcherMonitor = new MonitorRequest("model-trace", commandName);
            AddDispatcherMonitor(modelDispatcherMonitor);

            var dispatcherMonitor = new MonitorRequest("command-trace", commandName);
            AddDispatcherMonitor(dispatcherMonitor);

            var warningDispatcherMonitor = new MonitorRequest("warning-trace", commandName);
            AddDispatcherMonitor(warningDispatcherMonitor);

            var generalDispatcherMonitor = new MonitorRequest("general-trace", commandName);
            AddDispatcherMonitor(generalDispatcherMonitor);
        }

        /// <summary>
        ///     Add hooks and monitor to all traces with a default Console.Write method to print the trace
        /// </summary>
        public void StartTraceMonitoring()
        {
            StartTraceMonitoring(list =>
                Console.WriteLine("{0}: {1}", (string) list[1], ((string) list[2]).Replace("\n", "")));
        }

        /// <summary>
        ///     Remove Monitors and hooks from the Dispatcher, which are use to monitor the ACT-R traces
        ///     <see cref="StartTraceMonitoring()" />
        /// </summary>
        public void StopTraceMonitoring()
        {
            var commandName = ToString() + "_TraceMonitor";
            RemoveDispatcherMonitor("model-trace", commandName);
            RemoveDispatcherMonitor("command-trace", commandName);
            RemoveDispatcherMonitor("warning-trace", commandName);
            RemoveDispatcherMonitor("general-trace", commandName);
            RemoveDispatcherCommand("printTrace");
        }

        /// <summary>
        ///     Reset the ACT-R scheduler and all models.
        /// </summary>
        public void Reset()
        {
            SendMessage("evaluate", new object[] {"reset"});
        }

        /// <summary>
        ///     Sends an evaluation request to the Dispatcher
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result of the evaluated request</returns>
        public Result SendEvaluationRequest(AbstractEvaluationRequest request)
        {
            return SendMessage("evaluate", request.Parameterlist);
        }

        /// <summary>
        ///     Wait until a repsond from the dispatcher is received.
        /// </summary>
        /// <param name="id">JSON-RPC message id</param>
        /// <returns>Received respond</returns>
        private Result WaitForResult(int id)
        {
            while (_running)
            {
                Result result;
                if (_resultQueue.TryRemove(id, out result)) return result;
                Thread.Sleep(1);
            }

            // should never be executed
            return new Result
            {
                Id = id,
                AllRetruns = null,
                Error = new Error("Client is closing!")
            };
        }

        /// <summary>
        ///     Establish the TCP connection to the dispatcher
        /// </summary>
        private void StartTcpConnection()
        {
            _socket = new TcpClient();
            _socket.Connect(_host, _port);
            _streamReader = new StreamReader(_socket.GetStream());
            _streamWriter = new StreamWriter(_socket.GetStream());
        }

        /// <summary>
        ///     Starts a new thrad for filling the queues with received messages
        /// </summary>
        private void StartReceivingThread()
        {
            _incomingTask = new Task(() =>
            {
                var tmp = "";
                while (_running)
                {
                    try
                    {
                        char readChar;
                        readChar = (char) _streamReader.Read();
    
                        if (readChar.Equals('\x04'))
                        {
                            if (tmp.Contains("\"result\":"))
                            {
                                var result = JsonConvert.DeserializeObject<Result>(tmp);
                                _resultQueue.TryAdd(result.Id, result);
                            }
                            else
                            {
                                _messageQueueIncoming.Add(JsonConvert.DeserializeObject<Message>(tmp));
                            }
    
                            tmp = "";
                        }
                        else
                        {
                            tmp += readChar;
                        }
                    }
                    catch (IOException ){} // Socket is closed; Client is closing
                }
            });
            _incomingTask.Start(TaskScheduler.Default);
        }

        /// <summary>
        ///     Starts a new thread for evaluating commands form the dispatcher
        /// </summary>
        private void StartEvaluatingThread()
        {
            _evaluateTask = new Task(() =>
            {
                while (_running)
                {
                    try
                    {
                        var msg = _messageQueueIncoming.Take(_cancellationTokenSource.Token);
                        switch (msg.Method)
                        {
                            case "evaluate":
                                Evaluate(msg);
                                break;
                            default:
                                SendErrorResult(msg.Id, "Method not found: " + msg.Method);
                                break;
                        }
                    }
                    catch (OperationCanceledException){} // Client is closing
                }
            });
            _evaluateTask.Start(TaskScheduler.Default);
        }

        /// <summary>
        ///     Evaluates a command from the dispatcher
        /// </summary>
        /// <param name="msg"></param>
        private void Evaluate(Message msg)
        {
            try
            {
                _abstractCommands[(string) msg.Parameters[0]].Execute(msg.Parameters);
                SendSuccessResult(msg.Id);
            }
            catch (KeyNotFoundException)
            {
                SendErrorResult(msg.Id, "Command not found: " + (string) msg.Parameters[0]);
            }
        }

        /// <summary>
        ///     Starts a new thread for sending messages to the dispatcher
        /// </summary>
        private void StartOutgoingThread()
        {
            _outgoingTask = new Task(() =>
            {
                while (_running)
                {
                    try
                    {
                        var msg = _messageQueueOutgoing.Take(_cancellationTokenSource.Token);
                        if (msg.GetType() == typeof(Message))
                            _streamWriter.Write(((Message) msg).ToJson());
                        if (msg.GetType() == typeof(Result))
                            _streamWriter.Write(((Result) msg).ToJson());
                        _streamWriter.Flush();
                    }
                    catch (OperationCanceledException){} // Client is closing
                }
            });
            _outgoingTask.Start(TaskScheduler.Default);
        }


        /// <summary>
        ///     Sends a message to the dispatcher
        /// </summary>
        /// <param name="method">Method of the message</param>
        /// <param name="parameters">Parameter of the message</param>
        /// <returns>Received response of the dispathcer</returns>
        /// <exception cref="InvalidOperationException">Thrown when the dispacther has encounterd an error</exception>
        private Result SendMessage(string method, object[] parameters)
        {
            var commandMessage = new Message
            {
                Id = _idCount++,
                Method = method,
                Parameters = parameters
            };
            _messageQueueOutgoing.Add(commandMessage);
            var result = WaitForResult(commandMessage.Id);
            if (result.Error != null) throw new InvalidOperationException(result.Error.Message);
            return result;
        }

        /// <summary>
        ///     Sends an successful response to the dispatcher
        /// </summary>
        /// <param name="id">Id of the origin message</param>
        private void SendSuccessResult(int id)
        {
            var result = new Result
            {
                Id = id,
                AllRetruns = new object[] {true},
                Error = null
            };
            _messageQueueOutgoing.Add(result);
        }

        /// <summary>
        ///     Sends an eroor response to the dispatcher
        /// </summary>
        /// <param name="id">Id of the origin message</param>
        /// <param name="error">Error message</param>
        private void SendErrorResult(int id, string error)
        {
            var result = new Result
            {
                Id = id,
                AllRetruns = null,
                Error = new Error(error)
            };
            _messageQueueOutgoing.Add(result);
        }

        /// <summary>
        ///     Loads the ACT-R model file indicated.
        /// </summary>
        /// <param name="path">Path to ACT-R model file</param>
        public void LoadActrModel(string path)
        {
            SendEvaluationRequest(new LoadActrModel(path));
        }

        /// <summary>
        ///     Loads the Lisp code file indicated.
        /// </summary>
        /// <param name="path">Path to Lisp code file</param>
        public void LoadActrCode(string path)
        {
            SendEvaluationRequest(new LoadActrCode(path));
        }

        /// <summary>
        ///     Return a randomly ordered copy of the list provided using the current model's random stream.
        /// </summary>
        /// <param name="list">List, which sould be randomized</param>
        /// <returns>Randomized List</returns>
        public object[] PermuteList(object[] list)
        {
            return SendEvaluationRequest(new PermuteList(list)).ReturnList;
        }

        /// <summary>
        ///     Create a window for a task.
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="isVisible">Indicates, if the window should be visible</param>
        /// <param name="width">Window withd. Default: 300</param>
        /// <param name="height">Window height. Default: 300</param>
        /// <param name="x">Window X-Coordinate. Default: 300</param>
        /// <param name="y">Window Y-Coordinate. Default: 300</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public Window OpenExpWindow(string title, bool isVisible, int width = 300, int height = 300, int x = 300,
            int y = 300,
            string model = null)
        {
            var returnValues =
                SendEvaluationRequest(new OpenExpWindow(title, isVisible, width, height, x, y, model))
                    .ReturnList;
            return new Window((string) returnValues[0], (string) returnValues[1], (string) returnValues[2]);
        }

        /// <summary>
        ///     Create a text item for the provided experiment window with the features specified and place it in the window.
        /// </summary>
        /// <param name="window">Representation of the window</param>
        /// <param name="text">Text to place in the window</param>
        /// <param name="x">Text X-Coordinate</param>
        /// <param name="y">Text Y-Coordinate</param>
        /// <param name="color">Text color. Default: "black"</param>
        /// <param name="height">Text height. Default: 50</param>
        /// <param name="width">Text width. Default: 75</param>
        /// <param name="fontSize">Text size. Default: 12</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public Text AddTextToWindow(Window window, string text, int x, int y, string color = "black", int height = 50,
            int width = 75,
            int fontSize = 12, string model = null)
        {
            var returnValues = SendEvaluationRequest(new AddTextToWindow(window, text, x, y, color, height, width,
                fontSize,
                model)).ReturnList;
            return new Text((string) returnValues[0], (string) returnValues[1]);
        }

        /// <summary>
        ///     Create a button item for the provided experiment window with the features specified and place it in the window.
        /// </summary>
        /// <param name="window">Representation of the window</param>
        /// <param name="text">Text of the button</param>
        /// <param name="x">Button X-Coordinate</param>
        /// <param name="y">Button Y-Coordinate</param>
        /// <param name="action">Command, which should be executed if button is hit. Default: null</param>
        /// <param name="height">Button height. Default: 50</param>
        /// <param name="width">Button width. Default: 75</param>
        /// <param name="color">Button color. Default: "grey"</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void AddButtonToExpWindow(Window window, string text, int x, int y, object[] action = null,
            int height = 50,
            int width = 75,
            string color = "gray", string model = null)
        {
            SendEvaluationRequest(new AddButtonToExpWindow(window, text, x, y, action, height, width, color,
                model));
        }

        /// <summary>
        ///     Create a line for the provided experiment window with the features specified and then place it in that window.
        /// </summary>
        /// <param name="window">Representation of the window</param>
        /// <param name="start">X- and Y-Coordinates of the startpoint</param>
        /// <param name="end">X- and Y-Coordinates of the endpoint</param>
        /// <param name="color">Line color. Default: null</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public Line AddLineToExpWindow(Window window, int[] start, int[] end, string color = null,
            string model = null)
        {
            var returnValues = SendEvaluationRequest(new AddLineToExpWindow(window, start, end, color,
                model)).ReturnList;
            return new Line((string) returnValues[0], (string) returnValues[1]);
        }

        /// <summary>
        ///     Change the attributes of a line that was created for an experiment window.
        /// </summary>
        /// <param name="line">Representation of the line, which should be modified</param>
        /// <param name="start">X- and Y-Coordinates of the new startpoint</param>
        /// <param name="end">X- and Y-Coordinates of the new endpoint</param>
        /// <param name="color">New Line color. Default: null</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ModifyLineForExpWindow(Line line, int[] start, int[] end, string color = null,
            string model = null)
        {
            SendEvaluationRequest(new ModifyLineForExpWindow(line, start, end, color,
                model));
        }

        /// <summary>
        ///     Remove the given items from the window provided.
        /// </summary>
        /// <param name="window">Representation of the window</param>
        /// <param name="item">Item to remove from window</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RemoveItemFromExpWindow(Window window, IItem item,
            string model = null)
        {
            var removeItemsFromExpWindow = new RemoveItemFromExpWindow(window, item, model);
            SendEvaluationRequest(removeItemsFromExpWindow);
        }

        /// <summary>
        ///     Remove all items from the window provided.
        /// </summary>
        /// <param name="window">Representation of the window</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ClearExpWindow(Window window, string model = null)
        {
            ClearExpWindow(window.Title, model);
        }

        /// <summary>
        ///     Remove all items from the window provided.
        /// </summary>
        /// <param name="windowTitle">Title of the window</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ClearExpWindow(string windowTitle, string model = null)
        {
            SendEvaluationRequest(new ClearExpWindow(windowTitle, model));
        }

        /// <summary>
        ///     Compute the correlation between two lists of numbers.
        /// </summary>
        /// <param name="results">First list of data (normally the result of the simulation)</param>
        /// <param name="data">Second list of data (normally the data of a realy world experiment)</param>
        /// <param name="output">Indicates if the computed value should be written to the output stream. Default: true</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public double Correlation(List<double> results, List<double> data, bool output = true,
            string model = null)
        {
            return SendEvaluationRequest(new Correlation(results, data, output, model)).ReturnDouble;
        }

        /// <summary>
        ///     Compute the RMSD between two lists of numbers.
        /// </summary>
        /// <param name="results">First list of data (normally the result of the simulation)</param>
        /// <param name="data">Second list of data (normally the data of a realy world experiment)</param>
        /// <param name="output">Indicates if the computed value should be written to the output stream. Default: true</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public double MeanDeviation(List<double> results, List<double> data, bool output = true,
            string model = null)
        {
            return SendEvaluationRequest(new MeanDeviation(results, data, output, model)).ReturnDouble;
        }

        /// <summary>
        ///     Return a random number up to limit using the current model's random stream.
        /// </summary>
        /// <param name="value">Upper Limit</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns></returns>
        public long ActrRandom(long value, string model = null)
        {
            return SendEvaluationRequest(new ActrRandom(value, model)).ReturnLong;
        }

        /// <summary>
        ///     Command to install a device for an interface.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void InstallDevice(IDevice device, string model = null)
        {
            SendEvaluationRequest(new InstallDevice(device, model));
        }

        /// <summary>
        ///     Run the ACT-R scheduler for up to a specified amount of time.
        /// </summary>
        /// <param name="time">Time for the ACT-R scheduler to run</param>
        /// <param name="realTime">Indicates if the ACT-R scheduler should be running in real time. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Run(int time, bool realTime = false, string model = null)
        {
            AbstractEvaluationRequest abstractEvaluationRequest = new Run(time, realTime, model);
            SendEvaluationRequest(abstractEvaluationRequest);
        }

        /// <summary>
        ///     Create a new pure tone sound event for the audio module.
        /// </summary>
        /// <param name="frequence">Frequence of the tone</param>
        /// <param name="duration">Duration of the tone</param>
        /// <param name="onset">Time after the tone sould be created. Default: null</param>
        /// <param name="timeInMs">Indicates of times are given in milliseconds. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void NewToneSound(int frequence, double duration, double? onset = null, bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewToneSound(frequence, duration, onset, timeInMs, model));
        }

        /// <summary>
        ///     Create a new word sound event for the audio module.
        /// </summary>
        /// <param name="word">Word sound which should be created</param>
        /// <param name="onset">Time after the tone sould be created. Default: null</param>
        /// <param name="location">Default: "external"</param>
        /// <param name="timeInMs">Indicates of times are given in milliseconds. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void NewWordSound(string word, double? onset = null, string location = "external", bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewWordSound(word, onset, location, timeInMs, model));
        }

        /// <summary>
        ///     Create a new digit sound event for the audio module.
        /// </summary>
        /// <param name="digit">Digit sound which should be created</param>
        /// <param name="onset">Time after the tone sould be created. Default: null</param>
        /// <param name="timeInMs">Indicates of times are given in milliseconds. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void NewDigitSound(long digit, double? onset = null, bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewDigitSound(digit, onset, timeInMs, model));
        }

        /// <summary>
        ///     Create an event to occur after the specified time in milliseconds have passed.
        /// </summary>
        /// <param name="timeDelay">Time in milliseconds after that the event should be created</param>
        /// <param name="action">Name of the action that should be exectuted when the event is created</param>
        /// <param name="parameters">List of parameters for the action</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="priority">Priority of the scheduled event</param>
        /// <param name="maintenance">Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleEventRelative(long timeDelay, string action, object[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventRelative(timeDelay, action, parameters, module, priority,
                maintenance, model));
        }

        /// <summary>
        ///     Create an event to occur at the current time.
        /// </summary>
        /// <param name="action">Name of the action that should be exectuted when the event is created</param>
        /// <param name="parameters">List of parameters for the action</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="priority">Priority of the scheduled event</param>
        /// <param name="maintenance">Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleEventNow(string action, object[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventNow(action, parameters, module, priority,
                maintenance, model));
        }

        /// <summary>
        ///     Create an event to occur at the specified time in milliseconds.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="action">Name of the action that should be exectuted when the event is created</param>
        /// <param name="parameters">List of parameters for the action</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="priority">Priority of the scheduled event</param>
        /// <param name="maintenance">Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleEvent(long time, string action, object[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEvent(time, action, parameters, module, priority,
                maintenance, model));
        }

        /// <summary>
        ///     Reload the last ACT-R model file which was loaded
        /// </summary>
        /// <param name="compile">Default: false</param>
        public void Reload(bool compile = false)
        {
            SendEvaluationRequest(new Reload(compile));
        }

        /// <summary>
        ///     Run the ACT-R scheduler for up to a specified amount of time.
        /// </summary>
        /// <param name="time">Time for the ACT-R scheduler to run</param>
        /// <param name="realTime">Indicates if the ACT-R scheduler should be running in real time. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RunFullTime(int time, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunFullTime(time, realTime, model));
        }

        /// <summary>
        ///     Run the ACT-R scheduler up to the indicated time.
        /// </summary>
        /// <param name="time">Time for the ACT-R scheduler to stop at</param>
        /// <param name="realTime">Indicates if the ACT-R scheduler should be running in real time. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RunUntilTime(int time, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunUntilTime(time, realTime, model));
        }

        /// <summary>
        ///     Run the indicated number of events from the ACT-R scheduler.
        /// </summary>
        /// <param name="eventCount">Numberr of events, that the ACT-R scheduler should be running</param>
        /// <param name="realTime">Indicates if the ACT-R scheduler should be running in real time. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RunNEvents(long eventCount, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunNEvents(eventCount, realTime, model));
        }

        /// <summary>
        ///     Run the ACT-R scheduler until the provided function returns a non-nil value.
        /// </summary>
        /// <param name="condition">Function, which is evaluated after each event</param>
        /// <param name="realTime">Indicates if the ACT-R scheduler should be running in real time. Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RunUntilCondition(string condition, bool realTime = false,
            string model = null)
        {
            SendEvaluationRequest(new RunUntilCondition(condition, realTime, model));
        }

        /// <summary>
        ///     Print the contents of specified buffers.
        /// </summary>
        /// <param name="bufferNames">Buffer names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void BufferChunk(List<string> bufferNames, string model = null)
        {
            SendEvaluationRequest(new BufferChunk(bufferNames, model));
        }

        /// <summary>
        ///     Print the status information for a buffer and its module.
        /// </summary>
        /// <param name="bufferNames">Buffer names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of status infomartion of each specified buffer</returns>
        public object[] BufferStatus(List<string> bufferNames, string model = null)
        {
            return SendEvaluationRequest(new BufferStatus(bufferNames, model)).ReturnList;
        }

        /// <summary>
        ///     Return the name of the chunk in a buffer.
        /// </summary>
        /// <param name="bufferName">Buffer name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Chunk name of the chunk in the specified buffer</returns>
        public string BufferRead(string bufferName, string model = null)
        {
            return SendEvaluationRequest(new BufferRead(bufferName, model)).ReturnString;
        }

        /// <summary>
        ///     Clear the chunk from a buffer.
        /// </summary>
        /// <param name="bufferName">Buffer name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ClearBuffer(string bufferName, string model = null)
        {
            SendEvaluationRequest(new ClearBuffer(bufferName, model));
        }

        /// <summary>
        ///     Print the production matching details for specified productions.
        /// </summary>
        /// <param name="productionNames">Productions names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Whynot(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Whynot(productionNames, model));
        }

        /// <summary>
        ///     Print the declarative memory matching information based on the last retrieval request.
        /// </summary>
        /// <param name="chunkNames">Chunk names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void WhynotDm(List<string> chunkNames, string model = null)
        {
            SendEvaluationRequest(new WhynotDm(chunkNames, model));
        }

        /// <summary>
        ///     Restore the specified productions which were disabled.
        /// </summary>
        /// <param name="productionNames">Productions names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Penable(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Penable(productionNames, model));
        }

        /// <summary>
        ///     Prevent the specified productions from being selected.
        /// </summary>
        /// <param name="productionNames">Productions names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Pdisable(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Pdisable(productionNames, model));
        }

        /// <summary>
        ///     Schedule a chunk to enter the goal buffer at the current time or print the current goal buffer chunk.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void GoalFocus(string chunkName, string model = null)
        {
            SendEvaluationRequest(new GoalFocus(chunkName, model));
        }

        /// <summary>
        ///     Send a string to the ACT-R warning-trace.
        /// </summary>
        /// <param name="warning">Warning to print</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintWarning(string warning, string model = null)
        {
            SendEvaluationRequest(new PrintWarning(warning, model));
        }

        /// <summary>
        ///     Send a string to the ACT-R general-trace.
        /// </summary>
        /// <param name="output">Output to print</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ActrOutput(string output, string model = null)
        {
            SendEvaluationRequest(new ActrOutput(output, model));
        }

        /// <summary>
        ///     Print all the attributes of the features in the model's visicon.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintVisicon(string model = null)
        {
            SendEvaluationRequest(new PrintVisicon(model));
        }

        /// <summary>
        ///     Get current absolute model time or relative real time in milliseconds.
        /// </summary>
        /// <param name="modelTime">Indicates if model time should be returned. Default: true</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Time in milliseconds</returns>
        public long GetTime(bool modelTime = true, string model = null)
        {
            return SendEvaluationRequest(new GetTime(modelTime, model)).ReturnLong;
        }

        /// <summary>
        ///     Create chunks in the current model.
        /// </summary>
        /// <param name="chunks">Chunk descriptions</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void DefineChunks(object[] chunks, string model = null)
        {
            SendEvaluationRequest(new DefineChunks(chunks, model));
        }

        /// <summary>
        ///     Create chunks in the current model and add them to the model's declarative memory.
        /// </summary>
        /// <param name="chunks">Chunk descriptions</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void AddDm(object[] chunks, string model = null)
        {
            SendEvaluationRequest(new AddDm(chunks, model));
        }

        /// <summary>
        ///     Print the indicated chunks.
        /// </summary>
        /// <param name="chunkNames">Chunk names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PprintChunks(List<string> chunkNames, string model = null)
        {
            SendEvaluationRequest(new PprintChunks(chunkNames, model));
        }

        /// <summary>
        ///     Return the value of a slot in a chunk.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="slotName">Slot name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public object ChunkSlotValue(string chunkName, string slotName, string model = null)
        {
            return SendEvaluationRequest(new ChunkSlotValue(chunkName, slotName, model)).ReturnObject;
        }

        /// <summary>
        ///     Change the value of a slot in a chunk.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="slotName">Slot name</param>
        /// <param name="newValue">New slot value</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SetChunkSlotValue(string chunkName, object slotName, string newValue,
            string model = null)
        {
            SendEvaluationRequest(new SetChunkSlotValue(chunkName, (string) slotName, newValue, model));
        }

        /// <summary>
        ///     Modify the contents of the specified chunk with the list of slots and values provided.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="mods">Modificatcations as Sloat/Value pairs {slot-name new-slot-value}</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ModChunk(string chunkName, object[] mods, string model = null)
        {
            SendEvaluationRequest(new ModChunk(chunkName, mods, model));
        }

        /// <summary>
        ///     Modify the chunk in the goal buffer using the slots and values provided.
        /// </summary>
        /// <param name="mods">Modificatcations as Sloat/Value pairs {slot-name new-slot-value}</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ModFocus(object[] mods, string model = null)
        {
            SendEvaluationRequest(new ModFocus(mods, model));
        }

        /// <summary>
        ///     Returns whether the given name is the name of a chunk in the model
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ChunkP(string chunkName, string model = null)
        {
            SendEvaluationRequest(new ChunkP(chunkName, model));
        }

        /// <summary>
        ///     Returns the name of a chunk which is a copy of the chunk name provided.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void CopyChunk(string chunkName, string model = null)
        {
            SendEvaluationRequest(new CopyChunk(chunkName, model));
        }

        /// <summary>
        ///     Add a new slot name which can be used for any chunk, and if warn is true (the default) then print a warning if the
        ///     slot specified already exists.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="warn">Indicates if a warning should be printed if the slots already exsits</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ExtendPossibleSlots(string chunkName, bool warn = true, string model = null)
        {
            SendEvaluationRequest(new ExtendPossibleSlots(chunkName, warn, model));
        }

        /// <summary>
        ///     Send a string to the ACT-R model-trace.
        /// </summary>
        /// <param name="output">String to send to model-trace</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ModelOutput(string output, string model = null)
        {
            SendEvaluationRequest(new ModelOutput(output, model));
        }

        /// <summary>
        ///     Copy a chunk directly into a buffer.
        /// </summary>
        /// <param name="bufferName">Buffer name</param>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="requested"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SetBufferChunk(string bufferName, string chunkName, bool requested = true,
            string model = null)
        {
            SendEvaluationRequest(new SetBufferChunk(bufferName, chunkName, requested, model));
        }

        /// <summary>
        ///     Have the model place its right hand on the mouse before running.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void StartHandAtMouse(string model = null)
        {
            SendEvaluationRequest(new StartHandAtMouse(model));
        }

        /// <summary>
        ///     Print out a description of the events currently scheduled to occur optionally indicating which will show in the
        ///     trace.
        /// </summary>
        /// <param name="indicateTraced"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void MpShowQueue(bool indicateTraced, string model = null)
        {
            SendEvaluationRequest(new MpShowQueue(indicateTraced, model));
        }

        /// <summary>
        ///     Print a table showing any finsts marking recently retrieved chunks in declarative memory.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintDmFinsts(string model = null)
        {
            SendEvaluationRequest(new PrintDmFinsts(model));
        }

        /// <summary>
        ///     Set or get production parameters.
        /// </summary>
        /// <param name="parameters">See ACT-R manual</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>See ACT-R manual</returns>
        public object Spp(object[] parameters, string model = null)
        {
            return SendEvaluationRequest(new Spp(parameters, model)).ReturnObject;
        }

        /// <summary>
        ///     Return a list of all existing model names.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void MpModels(string model = null)
        {
            SendEvaluationRequest(new MpModels(model));
        }

        /// <summary>
        ///     "Returns a list of all the production names in the current model.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void AllProductions(string model = null)
        {
            SendEvaluationRequest(new AllProductions(model));
        }

        /// <summary>
        ///     Return a list with all the currently defined buffers' names.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Buffers(string model = null)
        {
            SendEvaluationRequest(new Buffers(model));
        }

        /// <summary>
        ///     Return a string with the printed output of all of the attributes of the features in the model's visicon.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Visicon features</returns>
        public string PrintedVisicon(string model = null)
        {
            return SendEvaluationRequest(new PrintedVisicon(model)).ReturnString;
        }

        /// <summary>
        ///     Print all the attributes of the features in the model's audicon to command output.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintAudicon(string model = null)
        {
            SendEvaluationRequest(new PrintAudicon(model));
        }

        /// <summary>
        ///     Return the string containing all the printed attributes of the features in the model's audicon.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Audicon features</returns>
        public string PrintedAudicon(string model = null)
        {
            return SendEvaluationRequest(new PrintedAudicon(model)).ReturnString;
        }

        /// <summary>
        ///     Returns a string of the parameter output for the given parameter's value and details.
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Parameter output</returns>
        public string PrintedParameterDetails(string parameterName, string model = null)
        {
            return SendEvaluationRequest(new PrintedParameterDetails(parameterName, model)).ReturnString;
        }

        /// <summary>
        ///     Returns a list of the names of all current modules sorted alphabetically.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of module names</returns>
        public object[] SortedModuleNames(string model = null)
        {
            return SendEvaluationRequest(new SortedModuleNames(model)).ReturnList;
        }

        /// <summary>
        ///     Returns a list of the names of all the parameters for the named module sorted alphabetically.
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of module parameters</returns>
        public object[] ModulesParameters(string moduleName, string model = null)
        {
            return SendEvaluationRequest(new ModulesParameters(moduleName, model)).ReturnList;
        }

        /// <summary>
        ///     Returns a list of the names of all current modules which provided parameters sorted alphabetically.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of modules</returns>
        public object[] ModulesWithParameters(string model = null)
        {
            return SendEvaluationRequest(new ModulesWithParameters(model)).ReturnList;
        }

        /// <summary>
        ///     Returns a list of all the buffers which are used in the productions of the current model.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of buffer names</returns>
        public object[] UsedProductionBuffers(string model = null)
        {
            return SendEvaluationRequest(new UsedProductionBuffers(model)).ReturnList;
        }

        /// <summary>
        ///     Start recording the named history information if not already being recorded.
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void RecordHistory(string historyName, string model = null)
        {
            SendEvaluationRequest(new RecordHistory(historyName, model));
        }

        /// <summary>
        ///     Stop recording the named history information if all requested starts have stopped.
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void StopRecordingHistory(string historyName, string model = null)
        {
            SendEvaluationRequest(new StopRecordingHistory(historyName, model));
        }

        /// <summary>
        ///     Return the indicated history stream data from the specfied file or current data using indicated parameters if no
        ///     file given.
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="fileName">File name or null. Default: null</param>
        /// <param name="parameters">Additional parameters. Default: null</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>History data</returns>
        public string GetHistoryData(string historyName, string fileName = null, object[] parameters = null,
            string model = null)
        {
            return SendEvaluationRequest(new GetHistoryData(historyName, fileName, parameters, model)).ReturnString;
        }

        /// <summary>
        ///     Return the result of applying the named processor to the history data from the specfied file or current data using
        ///     indicated parameters if no file given.
        /// </summary>
        /// <param name="processorName">Processor name</param>
        /// <param name="fileName">File name or null. Default: null</param>
        /// <param name="parameters">Additional parameters. Default: null</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Result of applyng the processor</returns>
        public string ProcessHistoryData(string processorName, string fileName = null, object[] parameters = null,
            string model = null)
        {
            return SendEvaluationRequest(new ProcessHistoryData(processorName, fileName, parameters,
                model)).ReturnString;
        }

        /// <summary>
        ///     Save the current history data from the indicated history stream and parameters to the specifed file.
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="fileName">File name or null. Default: null</param>
        /// <param name="parameters">Additional parameters. Default: null</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SaveHistoryData(string historyName, string fileName, object[] parameters,
            string model = null)
        {
            SendEvaluationRequest(new SaveHistoryData(historyName, fileName, parameters, model));
        }

        /// <summary>
        ///     Print the representation of the chunks from declarative memory with the given names.
        /// </summary>
        /// <param name="chunkNames">Chunk names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Dm(List<string> chunkNames, string model = null)
        {
            SendEvaluationRequest(new Dm(chunkNames, model));
        }

        /// <summary>
        ///     Print the chunks from declarative memory which match with the given specification.
        /// </summary>
        /// <param name="specifications">{({modifier} &lt;slot-name&gt; &lt;slot-value&gt;),...}</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Sdm(object[] specifications, string model = null)
        {
            SendEvaluationRequest(new Sdm(specifications, model));
        }

        /// <summary>
        ///     Return the current value of a parameter in the current model.
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Current value of the given parameter</returns>
        public object GetParameterValue(string parameterName, string model = null)
        {
            return SendEvaluationRequest(new GetParameterValue(parameterName, model)).ReturnObject;
        }

        /// <summary>
        ///     Sets the current value of a parameter in the current model.
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="newValue">New value for the given parameter</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SetParameterValue(string parameterName, object newValue, string model = null)
        {
            SendEvaluationRequest(new SetParameterValue(parameterName, newValue, model));
        }

        /// <summary>
        ///     Return the current value of a system parameter. Params: system-parameter-name
        /// </summary>
        /// <param name="systemParameterName">System parameter name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Current value of the given system parameter</returns>
        public object GetSystemParameterValue(string systemParameterName, string model = null)
        {
            return SendEvaluationRequest(new GetSystemParameterValue(systemParameterName, model)).ReturnObject;
        }

        /// <summary>
        ///     Sets the current value of a system parameter.
        /// </summary>
        /// <param name="systemParameterName">System parameter name</param>
        /// <param name="newValue">New value for the given system parameter</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SetSystemParameterValue(string systemParameterName, object newValue, string model = null)
        {
            SendEvaluationRequest(new SetSystemParameterValue(systemParameterName, newValue, model));
        }

        /// <summary>
        ///     Set or get declarative parameters
        /// </summary>
        /// <param name="parameters">See ACT-R manual</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>See ACT-R manual</returns>
        public object Sdp(object[] parameters, string model = null)
        {
            return SendEvaluationRequest(new Sdp(parameters, model)).ReturnObject;
        }

        /// <summary>
        ///     Given the specification of a retrieval request, output the activation trace of what would happen if that request
        ///     were made now.
        /// </summary>
        /// <param name="requestDetails"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void SimulateRetrievalRequest(object[] requestDetails, string model = null)
        {
            SendEvaluationRequest(new SimulateRetrievalRequest(requestDetails, model));
        }

        /// <summary>
        ///     Returns a list of the lists of times and chunks for which activation data has been recorded.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>List of lists of times and chunks</returns>
        public object[] SavedActivationHistory(string model = null)
        {
            return SendEvaluationRequest(new SavedActivationHistory(model)).ReturnList;
        }

        /// <summary>
        ///     Prints out the activation trace from the saved data at the specified time.
        /// </summary>
        /// <param name="time">Time</param>
        /// <param name="ms">Indicates if time is in milliseconds. Default: true</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintActivationTrace(int time, bool ms = true, string model = null)
        {
            SendEvaluationRequest(new PrintActivationTrace(time, ms, model));
        }

        /// <summary>
        ///     Prints out the activation trace from the saved data for a specific chunk at the specified time.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="time">Time</param>
        /// <param name="ms">Indicates if time is in milliseconds. Default: true</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintChunkActivationTrace(string chunkName, int time, bool ms = true,
            string model = null)
        {
            SendEvaluationRequest(new PrintChunkActivationTrace(chunkName, time, ms, model));
        }

        /// <summary>
        ///     Prints the internal representation of the productions with the given names or all if none provided.
        /// </summary>
        /// <param name="productionNames">List of producation names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Pp(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Pp(productionNames, model));
        }

        /// <summary>
        ///     Provide a reward signal for utility learning.
        /// </summary>
        /// <param name="rewardValue">RewardValue</param>
        /// <param name="maintenance"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void TriggerReward(long rewardValue, bool maintenance = false, string model = null)
        {
            SendEvaluationRequest(new TriggerReward(rewardValue, maintenance, model));
        }

        /// <summary>
        ///     Create a chunk-spec and return its id.
        /// </summary>
        /// <param name="spec">{{{mod} slot value},...}</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public string DefineChunkSpec(object[] spec, string model = null)
        {
            return SendEvaluationRequest(new DefineChunkSpec(spec, model)).ReturnString;
        }

        /// <summary>
        ///     Convert a chunk-spec id to a list of slot value pairs.
        /// </summary>
        /// <param name="chunkSpecId">Chunk-spec id</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ChunkSpecToChunkDef(string chunkSpecId, string model = null)
        {
            SendEvaluationRequest(new ChunkSpecToChunkDef(chunkSpecId, model));
        }

        /// <summary>
        ///     Release the chunk-spec associated with the provided id.
        /// </summary>
        /// <param name="chunkSpecId"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ReleaseChunkSpecId(string chunkSpecId, string model = null)
        {
            SendEvaluationRequest(new ReleaseChunkSpecId(chunkSpecId, model));
        }

        /// <summary>
        ///     Create an event to occur after the next event by a specified module.
        /// </summary>
        /// <param name="afterModule">After module</param>
        /// <param name="action">Name of the action that should be exectuted when the event is created</param>
        /// <param name="parameters">List of parameters for the action</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="maintenance">Default: false</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleEventAfterModule(string afterModule, string action, object[] parameters = null,
            string module = "NONE", bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventAfterModule(afterModule, action, parameters, module,
                maintenance, model));
        }

        /// <summary>
        ///     Schedule a chunk to be placed into a buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="chunk">Chunk to place</param>
        /// <param name="time">Time</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="priority">Priority of the scheduled event. Default: 0</param>
        /// <param name="requested"></param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleSetBufferChunk(string buffer, string chunk, int time,
            string module = "NONE", int priority = 0, bool requested = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleSetBufferChunk(buffer, chunk, time, module, priority,
                requested, model));
        }

        /// <summary>
        ///     Schedule the modification of a buffer chunk.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="modListOrSpec"></param>
        /// <param name="time">Time</param>
        /// <param name="module">Default: "NONE"</param>
        /// <param name="priority">Priority of the scheduled event. Default: 0</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void ScheduleSimpleModBufferChunk(string buffer, object[] modListOrSpec, int time,
            string module = "NONE", int priority = 0,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleModBufferChunk(buffer, modListOrSpec, time, module, priority,
                model));
        }

        /// <summary>
        ///     Remove the named module from the system.
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void UndefineModule(string moduleName, string model = null)
        {
            SendEvaluationRequest(new UndefineModule(moduleName, model));
        }

        /// <summary>
        ///     Delete a chunk from the model.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void DeleteChunk(string chunkName, string model = null)
        {
            SendEvaluationRequest(new DeleteChunk(chunkName, model));
        }

        /// <summary>
        ///     Delete a chunk from the model and release the name.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PurgeChunk(string chunkName, string model = null)
        {
            SendEvaluationRequest(new PurgeChunk(chunkName, model));
        }

        /// <summary>
        ///     Create a new module.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="buffers">Buffers</param>
        /// <param name="parameters">Parameter</param>
        /// <param name="version">Version</param>
        /// <param name="doc">Documentation</param>
        /// <param name="inter">Interface</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void DefineModule(string name, object[] buffers, object[] parameters, string version,
            string doc, string inter, string model = null)
        {
            SendEvaluationRequest(new DefineModule(name, buffers, parameters, version, doc, inter, model));
        }

        /// <summary>
        ///     Indicate that a request has completed.
        /// </summary>
        /// <param name="chunkSpecId">Chunk-spec id</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void CompleteRequest(string chunkSpecId, string model = null)
        {
            SendEvaluationRequest(new CompleteRequest(chunkSpecId, model));
        }

        /// <summary>
        ///     Indicate that all requests for a buffer are complete.
        /// </summary>
        /// <param name="bufferName">Buffer name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void CompleteAllBufferRequests(string bufferName,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllBufferRequests(bufferName, model));
        }

        /// <summary>
        ///     Indicate that all requests for a module are complete.
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void CompleteAllModuleRequests(string moduleName,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllModuleRequests(moduleName, model));
        }

        /// <summary>
        ///     Send a string to the ACT-R command-trace
        /// </summary>
        /// <param name="commandOutput">String to put in the command-trace</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void CommandOutput(string commandOutput, string model = null)
        {
            SendEvaluationRequest(new CommandOutput(commandOutput, model));
        }

        /// <summary>
        ///     Returns the name of the chunk from which the given chunk was copied.
        /// </summary>
        /// <param name="chunkName">Chunk name</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public string ChunkCopiedFrom(string chunkName, string model = null)
        {
            return SendEvaluationRequest(new ChunkCopiedFrom(chunkName, model)).ReturnString;
        }

        /// <summary>
        ///     Return the current simulation time in seconds.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Simulation time in seconds</returns>
        public long MpTime(string model = null)
        {
            return SendEvaluationRequest(new MpTime(model)).ReturnLong;
        }

        /// <summary>
        ///     Return the current simulation time in milliseconds.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        /// <returns>Simulation time in milliseconds</returns>
        public long MpTimeMs(string model = null)
        {
            return SendEvaluationRequest(new MpTimeMs(model)).ReturnLong;
        }

        /// <summary>
        ///     Print the BOLD response data for the currently recorded buffers.
        /// </summary>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void PrintBoldResponseData(string model = null)
        {
            SendEvaluationRequest(new PrintBoldResponseData(model));
        }

        /// <summary>
        ///     Pbreak causes the scheduler to stop when the specified productions are selected.
        /// </summary>
        /// <param name="productionNames">Production names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Pbreak(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Pbreak(productionNames, model));
        }

        /// <summary>
        ///     Punbreak removes the break flag from the specified productions.
        /// </summary>
        /// <param name="productionNames">Production names</param>
        /// <param name="model">Indicates if a specific model is requierd. If null, the current model will be used. Default: null</param>
        public void Punbreak(List<string> productionNames, string model = null)
        {
            SendEvaluationRequest(new Punbreak(productionNames, model));
        }
    }
}