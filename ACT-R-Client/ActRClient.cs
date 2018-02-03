using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.DispatcherEvaluates;
using Nyctico.Actr.Client.DispatcherHooks;
using Nyctico.Actr.Client.DispatcherMonitors;

namespace Nyctico.Actr.Client
{
    public class ActRClient : IDisposable
    {
        private readonly ConcurrentDictionary<string, AbstractDispatcherHook> _abstractCommands =
            new ConcurrentDictionary<string, AbstractDispatcherHook>();

        private readonly string _host;
        private readonly BlockingCollection<CommandMessage> _messageQueue = new BlockingCollection<CommandMessage>();

        private readonly ConcurrentDictionary<string, DispatcherMonitor> _monitors =
            new ConcurrentDictionary<string, DispatcherMonitor>();

        private readonly int _port;
        private readonly ConcurrentDictionary<int, Result> _resultQueue = new ConcurrentDictionary<int, Result>();
        private Task _evaluateTask;
        private int _idCount;
        private Task _queueTask;
        private bool _running = true;
        private TcpClient _socket;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;

        public ActRClient(string host, int port)
        {
            _host = host;
            _port = port;
            _idCount = 1;

            StartTcpConnection();
            StartReceivingThread();
            StartEvaluatingThread();
        }

        public void Dispose()
        {
            _running = false;
            _streamReader.Close();
            _streamWriter.Close();
            _socket.Close();
        }

        public void AddDispatcherHook(AbstractDispatcherHook dispatcherHook)
        {
            var result = SendMessage("check", dispatcherHook.PublishedNameAsList);
            if (result.ReturnValue == null || result.ReturnValue?[0] == null)
            {
                SendMessage("add", dispatcherHook.ToParameterList());
                _abstractCommands.TryAdd(dispatcherHook.PrivateName, dispatcherHook);
            }
        }

        public void Remove(AbstractDispatcherHook dispatcherHook)
        {
            var result = SendMessage("check", dispatcherHook.PublishedNameAsList);
            if (result.ReturnValue != null && result.ReturnValue?[0] != null)
            {
                SendMessage("remove", dispatcherHook.PublishedNameAsList);
                _abstractCommands.TryRemove(dispatcherHook.PrivateName, out dispatcherHook);
            }
        }

        public void RemoveCommand(string publishedName)
        {
            Remove(_abstractCommands[publishedName]);
        }

        public void AddDispatcherMonitor(DispatcherMonitor dispatcherMonitor)
        {
            SendMessage("monitor", dispatcherMonitor.ToParameterList());
            _monitors.TryAdd(dispatcherMonitor.CommandToMonitor + dispatcherMonitor.CommandToCall, dispatcherMonitor);
        }

        public void Remove(DispatcherMonitor dispatcherMonitor)
        {
            SendMessage("remove-monitor", dispatcherMonitor.ToParameterList());
            _monitors.TryRemove(dispatcherMonitor.CommandToMonitor + dispatcherMonitor.CommandToCall,
                out dispatcherMonitor);
        }

        public void RemoveMonitor(string commanToMonitorCommandToCall)
        {
            Remove(_monitors[commanToMonitorCommandToCall]);
        }

        public void StartTraceMonitoring(Action<List<dynamic>> traceAction)
        {
            var commandName = ToString() + "_TraceMonitor";
            AbstractDispatcherHook dispatcherHook =
                new LambdaDispatcherHook(traceAction, commandName, "printTrace", "Trace monitoring");
            AddDispatcherHook(dispatcherHook);

            var modelDispatcherMonitor = new DispatcherMonitor("model-trace", commandName);
            AddDispatcherMonitor(modelDispatcherMonitor);

            var dispatcherMonitor = new DispatcherMonitor("command-trace", commandName);
            AddDispatcherMonitor(dispatcherMonitor);

            var warningDispatcherMonitor = new DispatcherMonitor("warning-trace", commandName);
            AddDispatcherMonitor(warningDispatcherMonitor);

            var generalDispatcherMonitor = new DispatcherMonitor("general-trace", commandName);
            AddDispatcherMonitor(generalDispatcherMonitor);
        }

        public void StopTraceMonitoring()
        {
            var commandName = ToString() + "_TraceMonitor";
            RemoveMonitor("model-trace" + commandName);
            RemoveMonitor("command-trace" + commandName);
            RemoveMonitor("warning-trace" + commandName);
            RemoveMonitor("general-trace" + commandName);
            RemoveCommand("printTrace");
        }

        public void Reset()
        {
            SendMessage("evaluate", new List<dynamic> {"reset"});
        }

        public Result SendDispatcherEvaluate(AbstractEvalCommand evaluateCommand)
        {
            return SendMessage("evaluate", evaluateCommand.ToParameterList());
        }

        private Result WaitForResult(int id)
        {
            while (true)
            {
                if (_resultQueue.TryRemove(id, out var result)) return result;
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
                    var tmp = "";
                    while (true)
                    {
                        var readChar = (char) _streamReader.Read();
                        if (readChar.Equals('\x04'))
                        {
                            if (tmp.Contains("\"result\":"))
                            {
                                var result = JsonConvert.DeserializeObject<Result>(tmp);
                                _resultQueue.TryAdd(result.Id, result);
                            }
                            else
                            {
                                _messageQueue.Add(JsonConvert.DeserializeObject<CommandMessage>(tmp));
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
                    var msg = _messageQueue.Take();

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

        private void Evaluate(CommandMessage msg)
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


        private Result SendMessage(string method, List<dynamic> parameters)
        {
            var commandMessage = new CommandMessage
            {
                Id = _idCount++,
                Method = method,
                Parameters = parameters
            };
            _streamWriter.Write(commandMessage.ToJson());
            _streamWriter.Flush();
            var result = WaitForResult(commandMessage.Id);
            if (result.Error != null) throw new InvalidOperationException(result.Error.Message);
            return result;
        }

        private void SendSuccessResult(int id)
        {
            var result = new Result
            {
                Id = id,
                ReturnValue = new List<dynamic> {true},
                Error = null
            };
            _streamWriter.Write(result.ToJson());
            _streamWriter.Flush();
        }

        private void SendErrorResult(int id, string error)
        {
            var result = new Result
            {
                Id = id,
                ReturnValue = null,
                Error = new Error(error)
            };
            _streamWriter.Write(result.ToJson());
            _streamWriter.Flush();
        }

        public void LoadModel(string modelName)
        {
            SendMessage("evaluate", new List<dynamic> {"load-act-r-model", false, modelName});
        }
    }
}