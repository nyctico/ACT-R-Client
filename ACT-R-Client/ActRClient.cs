﻿using System;
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
            if (result.ReturnList != null && result.ReturnValue != null) return;
            SendMessage("add", dispatcherHook.ToParameterList());
            _abstractCommands.TryAdd(dispatcherHook.PrivateName, dispatcherHook);
        }

        public void RemoveDispatcherHook(AbstractDispatcherHook dispatcherHook)
        {
            var result = SendMessage("check", dispatcherHook.PublishedNameAsList);
            if (result.ReturnList == null || result.ReturnValue == null) return;
            SendMessage("remove", dispatcherHook.PublishedNameAsList);
            _abstractCommands.TryRemove(dispatcherHook.PrivateName, out dispatcherHook);
        }

        public void RemoveDispatcherHook(string privateName)
        {
            AbstractDispatcherHook abstractDispatcherHook;
            if (!_abstractCommands.TryRemove(privateName, out abstractDispatcherHook))
                throw new KeyNotFoundException("DispatcherHook not found!");
            RemoveDispatcherHook(abstractDispatcherHook);
        }

        public void AddDispatcherMonitor(DispatcherMonitor dispatcherMonitor)
        {
            SendMessage("monitor", dispatcherMonitor.ToParameterList());
            _monitors.TryAdd(dispatcherMonitor.CommandToMonitor + dispatcherMonitor.CommandToCall, dispatcherMonitor);
        }

        public void RemoveDispatcherMonitor(DispatcherMonitor dispatcherMonitor)
        {
            SendMessage("remove-monitor", dispatcherMonitor.ToParameterList());
            _monitors.TryRemove(dispatcherMonitor.CommandToMonitor + dispatcherMonitor.CommandToCall,
                out dispatcherMonitor);
        }

        public void RemoveDispatcherMonitor(string commanToMonitorCommandToCall)
        {
            DispatcherMonitor dispatcherMonitor;
            if (!_monitors.TryRemove(commanToMonitorCommandToCall, out dispatcherMonitor))
                throw new KeyNotFoundException("Monitor not found!");
            RemoveDispatcherMonitor(dispatcherMonitor);
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

        public void StartTraceMonitoring()
        {
            StartTraceMonitoring(list => Console.WriteLine(
                $"{(string) list[1]}: {((string) list[2]).Replace("\n", "")}"));
        }

        public void StopTraceMonitoring()
        {
            var commandName = ToString() + "_TraceMonitor";
            RemoveDispatcherMonitor("model-trace" + commandName);
            RemoveDispatcherMonitor("command-trace" + commandName);
            RemoveDispatcherMonitor("warning-trace" + commandName);
            RemoveDispatcherMonitor("general-trace" + commandName);
            RemoveDispatcherHook("printTrace");
        }

        public void Reset()
        {
            SendMessage("evaluate", new List<dynamic> {"reset"});
        }

        public Result SendDispatcherEvaluate(AbstractDispatcherEvaluate evaluateCommand)
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
                ReturnList = new List<dynamic> {true},
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
                ReturnList = null,
                Error = new Error(error)
            };
            _streamWriter.Write(result.ToJson());
            _streamWriter.Flush();
        }

        public void LoadModel(string modelName)
        {
            SendMessage("evaluate", new List<dynamic> {"load-act-r-model", false, modelName});
        }

        public List<dynamic> PermuteList(List<dynamic> list, bool useModel = false, string model = null)
        {
            return SendDispatcherEvaluate(new PermuteList(list, useModel, model)).ReturnValue.ToObject<List<dynamic>>();
        }

        public Device OpenExpWindow(string title, bool isVisible, int width = 300, int height = 300, int x = 300,
            int y = 300,
            bool useModel = false,
            string model = null)
        {
            return new Device(
                SendDispatcherEvaluate(new OpenExpWindow(title, isVisible, width, height, x, y, useModel, model))
                    .ReturnValue.ToObject<List<dynamic>>());
        }

        public void AddTextToWindow(Device window, string text, int x, int y, string color = "black", int height = 50,
            int width = 75,
            int fontSize = 12, bool useModel = false, string model = null)
        {
            SendDispatcherEvaluate(new AddTextToWindow(window, text, x, y, color, height, width, fontSize, useModel,
                model));
        }

        public double Correlation(List<dynamic> results, List<dynamic> data, bool output = true, bool useModel = false,
            string model = null)
        {
            return SendDispatcherEvaluate(new Correlation(results, data, output, useModel, model)).ReturnValue;
        }

        public double MeanDeviation(List<dynamic> results, List<dynamic> data, bool output = true,
            bool useModel = false,
            string model = null)
        {
            return SendDispatcherEvaluate(new MeanDeviation(results, data, output, useModel, model)).ReturnValue;
        }

        public long ActrRandom(long value, bool useModel = false, string model = null)
        {
            return SendDispatcherEvaluate(new ActrRandom(value, useModel, model)).ReturnValue;
        }

        public void InstallDevice(Device device, bool useModel = false, string model = null)
        {
            SendDispatcherEvaluate(new InstallDevice(device, useModel, model));
        }

        public void Run(int time, bool realTime, bool useModel = false, string model = null)
        {
            SendDispatcherEvaluate(new Run(time, realTime, useModel, model));
        }

        public void NewToneSound(int frequence, double duration, double? onset = null, bool timeInMs = false,
            bool useModel = false, string model = null)
        {
            SendDispatcherEvaluate(new NewToneSound(frequence, duration, onset, timeInMs, useModel, model));
        }

        public void ScheduleSimpleEventRelative(long timeDelay, string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, bool useModel = false,
            string model = null)
        {
            SendDispatcherEvaluate(new ScheduleSimpleEventRelative(timeDelay, action, parameters, module, priority,
                maintenance, useModel, model));
        }
    }
}