using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nyctico.Actr.Client.Commands;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.DispatcherCommands;

namespace Nyctico.Actr.Client
{
    public class ActRClient : IDisposable
    {
        private int _idCount;
        private TcpClient _socket;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private bool _running = true;
        private Task _evaluateTask;
        private Task _queueTask;

        private readonly string _host;
        private readonly int _port;
        private readonly BlockingCollection<Message> _messageQueue = new BlockingCollection<Message>();
        private readonly ConcurrentDictionary<int, Result> _resultQueue = new ConcurrentDictionary<int, Result>();
        private readonly ConcurrentDictionary<string, AbstractCommand> _abstractCommands = new ConcurrentDictionary<string, AbstractCommand>();
        private readonly ConcurrentDictionary<string, MonitorCommand> _monitors = new ConcurrentDictionary<string, MonitorCommand>();
        
        public ActRClient(string host, int port)
        {
            _host = host;
            _port = port;
            _idCount = 1;
            
            StartTcpConnection();
            StartReceivingThread();
            StartEvaluatingThread();
        }

        public void Add(AbstractCommand command)
        {
            Result result = SendMessage("check", command.PublishedNameAsList);
            if (result.ReturnValue == null || result.ReturnValue?[0] == null)
            {
                SendMessage("add", command.ToParameterList());
                _abstractCommands.TryAdd(command.PrivateName, command);
            }
        }

        public void Remove(AbstractCommand command)
        {
            Result result = SendMessage("check", command.PublishedNameAsList);
            if (result.ReturnValue != null && result.ReturnValue?[0] != null)
            {
                SendMessage("remove", command.PublishedNameAsList);
                _abstractCommands.TryRemove(command.PrivateName, out command);
            }
        }

        public void RemoveCommand(string publishedName)
        {
            Remove(_abstractCommands[publishedName]);
        }

        public void Add(MonitorCommand monitor)
        {
            SendMessage("monitor", monitor.ToParameterList());
            _monitors.TryAdd(monitor.CommandToMonitor + monitor.CommandToCall, monitor);
        }

        public void Remove(MonitorCommand monitor)
        {
            SendMessage("remove-monitor", monitor.ToParameterList());
            _monitors.TryRemove(monitor.CommandToMonitor + monitor.CommandToCall, out monitor);
        }

        public void RemoveMonitor(string commanToMonitorCommandToCall)
        {
            Remove(_monitors[commanToMonitorCommandToCall]);
        }

        public void StartTraceMonitoring(Action<List<dynamic>> traceAction)
        {
            string commandName = ToString() + "_TraceMonitor";
            AbstractCommand command = new LambdaCommand(traceAction, commandName, "printTrace", "Trace monitoring");
            Add(command);
            
            MonitorCommand modelMonitor = new MonitorCommand("model-trace", commandName);
            Add(modelMonitor);

            MonitorCommand commandMonitor = new MonitorCommand("command-trace", commandName);
            Add(commandMonitor);

            MonitorCommand warningMonitor = new MonitorCommand("warning-trace", commandName);
            Add(warningMonitor);

            MonitorCommand generalMonitor = new MonitorCommand("general-trace", commandName);
            Add(generalMonitor);
        }

        public void StopTraceMonitoring()
        {
            string commandName = ToString() + "_TraceMonitor";
            RemoveMonitor("model-trace"+commandName);
            RemoveMonitor("command-trace"+commandName);
            RemoveMonitor("warning-trace"+commandName);
            RemoveMonitor("general-trace"+commandName);
            RemoveCommand("printTrace");
        }

        public void Reset()
        {
            SendMessage("evaluate", new List<dynamic>{"reset"});
        }

        public Result SendDispatcherCommand(AbstractEvalCommand evaluateCommand)
        {
            return SendMessage("evaluate", evaluateCommand.ToParameterList());
        }

        private Result WaitForResult(int id)
        {
            while (true)
            {
                if (_resultQueue.TryRemove(id, out Result result))
                {
                    //Console.WriteLine(result.ToJson());
                    return result;
                }
                Thread.Sleep(1);
            }
        }

        private void StartTcpConnection()
        {
            _socket = new TcpClient();
            _socket.Connect(_host, _port);
            _streamReader = new StreamReader(_socket.GetStream());
            _streamWriter = new StreamWriter(_socket.GetStream());
        }

        private void StartReceivingThread()
        {
            _queueTask = new Task(() =>
            {
                while (_running)
                {
                    string tmp = "";
                    while (true)
                    {
                        char readChar = (char) _streamReader.Read();
                        if (readChar.Equals('\x04'))
                        {
                            if (tmp.Contains("\"result\":"))
                            {
                                Result result = JsonConvert.DeserializeObject<Result>(tmp);
                                _resultQueue.TryAdd(result.Id, result);
                            }
                            else
                            {
                                _messageQueue.Add(JsonConvert.DeserializeObject<Message>(tmp));
                            }
                            break;
                        }
                        tmp += readChar;
                    }
                }
            });
            _queueTask.Start();
        }

        private void StartEvaluatingThread()
        {
            _evaluateTask = new Task(() =>
            {
                while (_running)
                {
                    Message msg = _messageQueue.Take();
                    
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
            });
            _evaluateTask.Start();
        }

        private void Evaluate(Message msg)
        {
            try
            {
                _abstractCommands[(string)msg.Parameters[0]].Execute(msg.Parameters);
                SendSuccessResult(msg.Id);
            }
            catch (KeyNotFoundException)
            {
                SendErrorResult(msg.Id, "Command not found: " + (string)msg.Parameters[0]);
            }
        }

        
        private Result SendMessage(string method, List<dynamic> parameters)
        {
            Message message = new Message
            {
                Id = _idCount++,
                Method = method,
                Parameters = parameters
            };
            _streamWriter.Write(message.ToJson());
            _streamWriter.Flush();
            Result result = WaitForResult(message.Id);
            if (result.Error != null)
            {
                throw new InvalidOperationException(result.Error.Message);
            }
            return result;
        }
        
        private void SendSuccessResult(int id)
        {
            Result result = new Result
            {
                Id = id,
                ReturnValue = new List<dynamic>{true},
                Error = null
            };
            _streamWriter.Write(result.ToJson());
            _streamWriter.Flush();
        }
        
        private void SendErrorResult(int id, string error)
        {
            Result result = new Result
            {
                Id = id,
                ReturnValue = null,
                Error = new Error(error)
            };
            _streamWriter.Write(result.ToJson());
            _streamWriter.Flush();
        }

        public void Dispose()
        {
            _running = false;
            _streamReader.Close();
            _streamWriter.Close();
            _socket.Close();
        }

        public void LoadModel(string modelName)
        {
            SendMessage("evaluate", new List<dynamic> {"load-act-r-model", false, modelName});
        }
    }
}