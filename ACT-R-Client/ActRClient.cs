using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.EvaluationRequests;
using Nyctico.Actr.Client.HookRequests;
using Nyctico.Actr.Client.MonitorRequests;

namespace Nyctico.Actr.Client
{
    public class ActRClient : IDisposable
    {
        private readonly ConcurrentDictionary<string, AbstractHookRequest> _abstractCommands =
            new ConcurrentDictionary<string, AbstractHookRequest>();

        private readonly string _host;
        private readonly BlockingCollection<Hook> _messageQueue = new BlockingCollection<Hook>();

        private readonly ConcurrentDictionary<string, MonitorRequest> _monitors =
            new ConcurrentDictionary<string, MonitorRequest>();

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

        public void AddDispatcherHook(AbstractHookRequest hookRequest)
        {
            var result = SendMessage("check", hookRequest.PublishedNameAsList);
            if (result.ReturnList != null && result.ReturnValue != null) return;
            SendMessage("add", hookRequest.ToParameterList());
            _abstractCommands.TryAdd(hookRequest.PrivateName, hookRequest);
        }

        public void RemoveDispatcherHook(AbstractHookRequest hookRequest)
        {
            var result = SendMessage("check", hookRequest.PublishedNameAsList);
            if (result.ReturnList == null || result.ReturnValue == null) return;
            SendMessage("remove", hookRequest.PublishedNameAsList);
            _abstractCommands.TryRemove(hookRequest.PrivateName, out hookRequest);
        }

        public void RemoveDispatcherHook(string privateName)
        {
            AbstractHookRequest abstractHookRequest;
            if (!_abstractCommands.TryRemove(privateName, out abstractHookRequest))
                throw new KeyNotFoundException("DispatcherHook not found!");
            RemoveDispatcherHook(abstractHookRequest);
        }

        public void AddDispatcherMonitor(MonitorRequest monitorRequest)
        {
            SendMessage("monitor", monitorRequest.ToParameterList());
            _monitors.TryAdd(monitorRequest.CommandToMonitor + monitorRequest.CommandToCall, monitorRequest);
        }

        public void RemoveDispatcherMonitor(MonitorRequest monitorRequest)
        {
            SendMessage("remove-monitor", monitorRequest.ToParameterList());
            _monitors.TryRemove(monitorRequest.CommandToMonitor + monitorRequest.CommandToCall,
                out monitorRequest);
        }

        public void RemoveDispatcherMonitor(string commanToMonitorCommandToCall)
        {
            MonitorRequest monitorRequest;
            if (!_monitors.TryRemove(commanToMonitorCommandToCall, out monitorRequest))
                throw new KeyNotFoundException("Monitor not found!");
            RemoveDispatcherMonitor(monitorRequest);
        }

        public void StartTraceMonitoring(Action<List<dynamic>> traceAction)
        {
            var commandName = ToString() + "_TraceMonitor";
            AbstractHookRequest hookRequest =
                new LambdaHookRequest(traceAction, commandName, "printTrace", "Trace monitoring");
            AddDispatcherHook(hookRequest);

            var modelDispatcherMonitor = new MonitorRequest("model-trace", commandName);
            AddDispatcherMonitor(modelDispatcherMonitor);

            var dispatcherMonitor = new MonitorRequest("command-trace", commandName);
            AddDispatcherMonitor(dispatcherMonitor);

            var warningDispatcherMonitor = new MonitorRequest("warning-trace", commandName);
            AddDispatcherMonitor(warningDispatcherMonitor);

            var generalDispatcherMonitor = new MonitorRequest("general-trace", commandName);
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

        public Result SendEvaluationRequest(AbstractEvaluationRequest request)
        {
            return SendMessage("evaluate", request.ToParameterList());
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
                                _messageQueue.Add(JsonConvert.DeserializeObject<Hook>(tmp));
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

        private void Evaluate(Hook msg)
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
            var commandMessage = new Hook
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

        public void LoadActrModel(string path, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new LoadActrModel(path, useModel, model));
        }

        public void LoadActrCode(string path, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new LoadActrCode(path, useModel, model));
        }

        public List<dynamic> PermuteList(List<dynamic> list, bool useModel = false, string model = null)
        {
            return SendEvaluationRequest(new PermuteList(list, useModel, model)).ReturnValue.ToObject<List<dynamic>>();
        }

        public Window OpenExpWindow(string title, bool isVisible, int width = 300, int height = 300, int x = 300,
            int y = 300,
            bool useModel = false,
            string model = null)
        {
            List<dynamic> returnValue = SendEvaluationRequest(new OpenExpWindow(title, isVisible, width, height, x, y, useModel, model))
                .ReturnValue.ToObject<List<dynamic>>();
            return new Window(returnValue[0],returnValue[1],returnValue[2]);
        }

        public void AddTextToWindow(Window window, string text, int x, int y, string color = "black", int height = 50,
            int width = 75,
            int fontSize = 12, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new AddTextToWindow(window, text, x, y, color, height, width, fontSize, useModel,
                model));
        }

        public void AddButtonToExpWindow(Window window, string text, int x, int y, List<dynamic> action = null,
            int height = 50,
            int width = 75,
            string color = "gray", bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new AddButtonToExpWindow(window, text, x, y, action, height, width, color, useModel,
                model));
        }
        
        public Line AddLineToExpWindow(Window window, int[] start, int[] end, string color = null, bool useModel = false, string model = null)
        {
            var returnValue = SendEvaluationRequest(new AddLineToExpWindow(window, start, end, color, useModel,
                model)).ReturnValue.ToObject<List<dynamic>>();
            return new Line(returnValue[0],returnValue[1]);
        }
        
        public void ModifyLineForExpWindow(Line line, int[] start, int[] end, string color = null, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModifyLineForExpWindow(line, start, end, color, useModel,
                model));
        }

        public void RemoveItemFromExpWindow(Window window, IItem item, bool useModel = false,
            string model = null)
        {
            var removeItemsFromExpWindow = new RemoveItemFromExpWindow(window, item, useModel, model);
            SendEvaluationRequest(removeItemsFromExpWindow);
        }

        public void ClearExpWindow(Window window, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ClearExpWindow(window, useModel, model));
        }

        public double Correlation(List<dynamic> results, List<dynamic> data, bool output = true, bool useModel = false,
            string model = null)
        {
            return SendEvaluationRequest(new Correlation(results, data, output, useModel, model)).ReturnValue;
        }

        public double MeanDeviation(List<dynamic> results, List<dynamic> data, bool output = true,
            bool useModel = false,
            string model = null)
        {
            return SendEvaluationRequest(new MeanDeviation(results, data, output, useModel, model)).ReturnValue;
        }

        public long ActrRandom(long value, bool useModel = false, string model = null)
        {
            return SendEvaluationRequest(new ActrRandom(value, useModel, model)).ReturnValue;
        }

        public void InstallDevice(Window window, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new InstallDevice(window, useModel, model));
        }

        public void Run(int time, bool realTime = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Run(time, realTime, useModel, model));
        }

        public void NewToneSound(int frequence, double duration, double? onset = null, bool timeInMs = false,
            bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new NewToneSound(frequence, duration, onset, timeInMs, useModel, model));
        }

        public void NewWordSound(string word, double? onset = null, string location = "external", bool timeInMs = false,
            bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new NewWordSound(word, onset, location, timeInMs, useModel, model));
        }

        public void NewDigitSound(long digit, double? onset = null, bool timeInMs = false,
            bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new NewDigitSound(digit, onset, timeInMs, useModel, model));
        }

        public void ScheduleSimpleEventRelative(long timeDelay, string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventRelative(timeDelay, action, parameters, module, priority,
                maintenance, useModel, model));
        }
        
        public void ScheduleSimpleEventNow(string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventNow(action, parameters, module, priority,
                maintenance, useModel, model));
        }
        
        public void ScheduleSimpleEvent(long time, string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEvent(time, action, parameters, module, priority,
                maintenance, useModel, model));
        }

        public void Reload(bool compile = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Reload(compile));
        }

        public void RunFullTime(int time, bool realTime = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new RunFullTime(time, realTime, useModel, model));
        }

        public void RunUntilTime(int time, bool realTime = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new RunUntilTime(time, realTime, useModel, model));
        }

        public void RunNEvents(long eventCount, bool realTime = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new RunNEvents(eventCount, realTime, useModel, model));
        }

        public void RunUntilCondition(string condition, bool realTime = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new RunUntilCondition(condition, realTime, useModel, model));
        }

        public void BufferChunk(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new BufferChunk(parameters, useModel, model));
        }

        public List<dynamic> BufferStatus(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            return SendEvaluationRequest(new BufferStatus(parameters, useModel, model)).ReturnValue;
        }

        public List<dynamic> BufferRead(string buffer, bool useModel = false, string model = null)
        {
            return SendEvaluationRequest(new BufferRead(buffer, useModel, model)).ReturnValue;
        }

        public void ClearBuffer(string buffer, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ClearBuffer(buffer, useModel, model));
        }

        public void Whynot(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Whynot(parameters, useModel, model));
        }

        public void WhynotDm(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new WhynotDm(parameters, useModel, model));
        }

        public void Penable(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Penable(parameters, useModel, model));
        }

        public void Pdisable(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Pdisable(parameters, useModel, model));
        }

        public void GoalFocus(string goal = null, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new GoalFocus(goal, useModel, model));
        }

        public void PrintWarning(string warning, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintWarning(warning, useModel, model));
        }

        public void ActrOutput(string output, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ActrOutput(output, useModel, model));
        }

        public void PrintVisicon(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintVisicon(useModel, model));
        }

        public void GetTime(bool modelTime = true, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new GetTime(modelTime, useModel, model));
        }

        public void DefineChunks(List<dynamic> chunks, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new DefineChunks(chunks, useModel, model));
        }

        public void AddDm(List<dynamic> chunks, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new AddDm(chunks, useModel, model));
        }

        public void PprintChunks(List<dynamic> chunks, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PprintChunks(chunks, useModel, model));
        }

        public void ChunkSlotValue(string chunkName, string slotName, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ChunkSlotValue(chunkName, slotName, useModel, model));
        }

        public void SetChunkSlotValue(string chunkName, string slotName, string newValue, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new SetChunkSlotValue(chunkName, slotName, newValue, useModel, model));
        }

        public void ModChunk(string chunkName, List<dynamic> mods, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModChunk(chunkName, mods, useModel, model));
        }

        public void ModFocus(List<dynamic> mods, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModFocus(mods, useModel, model));
        }

        public void ChunkP(string chunkName, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ChunkP(chunkName, useModel, model));
        }

        public void CopyChunk(string chunkName, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new CopyChunk(chunkName, useModel, model));
        }

        public void ExtendPossibleSlots(string chunkName, bool warn = true, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ExtendPossibleSlots(chunkName, warn, useModel, model));
        }

        public void ModelOutput(string output, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModelOutput(output, useModel, model));
        }

        public void SetBufferChunk(string bufferName, string chunkName, bool requested = true, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new SetBufferChunk(bufferName, chunkName, requested, useModel, model));
        }
        
        public void StartHandAtMouse(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new StartHandAtMouse(useModel, model));
        }
        
        public void MpShowQueue(bool indicateTraced, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new MpShowQueue(indicateTraced, useModel, model));
        }
        
        public void PrintDmFinsts(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintDmFinsts(useModel, model));
        }
        
        public void Spp(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Spp(parameters, useModel, model));
        }
        
        public void MpModels(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new MpModels(useModel, model));
        }
        
        public void AllProductions(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new AllProductions(useModel, model));
        }
        
        public void Buffers(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Buffers(useModel, model));
        }
        
        public void PrintedVisicon(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintedVisicon(useModel, model));
        }
        
        public void PrintAudicon(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintAudicon(useModel, model));
        }
        
        public void PrintedAudicon(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintedAudicon(useModel, model));
        }
        
        public void PrintedParameterDetails(string parameter, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintedParameterDetails(parameter, useModel, model));
        }
        
        public void SortedModuleNames(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SortedModuleNames(useModel, model));
        }
        
        public void ModulesParameters(string module, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModulesParameters(module, useModel, model));
        }
        
        public void ModulesWithParameters(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ModulesWithParameters(useModel, model));
        }
        
        public void UsedProductionBuffers(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new UsedProductionBuffers(useModel, model));
        }
        
        public void RecordHistory(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new RecordHistory(parameters, useModel, model));
        }
        
        public void StopRecordingHistory(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new StopRecordingHistory(parameters, useModel, model));
        }
        
        public void GetHistoryData(string history, List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new GetHistoryData(history, parameters, useModel, model));
        }
        
        public void ProcessHistoryData(string processor, bool file, List<dynamic> dataParameters, List<dynamic> processorParameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ProcessHistoryData(processor,file, dataParameters,processorParameters, useModel, model));
        }
        
        public void SaveHistoryData(string history, bool file, string comments, List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SaveHistoryData(history,file, comments,parameters, useModel, model));
        }
        
        public void Dm(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Dm(parameters, useModel, model));
        }
        
        public void Sdm(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Sdm(parameters, useModel, model));
        }
        
        public void GetParameterValue(string parameter, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new GetParameterValue(parameter, useModel, model));
        }
        
        public void SetParameterValue(string parameter,string value, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SetParameterValue(parameter, value, useModel, model));
        }
        
        public void GetSystemParameterValue(string parameter, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new GetSystemParameterValue(parameter, useModel, model));
        }
        
        public void SetSystemParameterValue(string parameter,string value, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SetSystemParameterValue(parameter, value, useModel, model));
        }
        
        public void Sdp(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Sdp(parameters, useModel, model));
        }
        
        public void SimulateRetrievalRequest(List<dynamic> spec, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SimulateRetrievalRequest(spec, useModel, model));
        }
        
        public void SavedActivationHistory(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new SavedActivationHistory(useModel, model));
        }
        
        public void PrintActivationTrace(int time, bool ms = true, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintActivationTrace(time, ms, useModel, model));
        }
        
        public void PrintChunkActivationTrace(string chunkName, int time, bool ms=true, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintChunkActivationTrace(chunkName, time, ms, useModel, model));
        }
        
        public void Pp(List<dynamic> parameters, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Pp(parameters, useModel, model));
        }
        
        public void TriggerReward(string reward,bool maintenance = false, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new TriggerReward(reward, maintenance, useModel, model));
        }
        
        public void DefineChunkSpec(List<dynamic> spec, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new DefineChunkSpec(spec, useModel, model));
        }
        
        public void ChunkSpecToChunkDef(string specId, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ChunkSpecToChunkDef(specId, useModel, model));
        }
        
        public void ReleaseChunkSpecId(string specId, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ReleaseChunkSpecId(specId, useModel, model));
        }
        
        public void ScheduleSimpleEventAfterModule(string afterModule, string action, List<dynamic> parameters = null,
            string module = "NONE", bool maintenance = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventAfterModule(afterModule, action, parameters, module,
                maintenance, useModel, model));
        }
        
        public void ScheduleSimpleSetBufferChunk(string buffer, string chunk, int time,
            string module = "NONE", int priority = 0, bool requested = false, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleSetBufferChunk(buffer,chunk,time, module, priority,
                requested, useModel, model));
        }
        
        public void ScheduleSimpleModBufferChunk(string buffer,List<dynamic> modListOrSpec, int time,
            string module = "NONE", int priority = 0, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleModBufferChunk(buffer,modListOrSpec,time, module, priority,
                useModel, model));
        }
        
        public void UndefineModule(string name, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new UndefineModule(name, useModel, model));
        }
        
        public void DeleteChunk(string name, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new DeleteChunk(name, useModel, model));
        }
        
        public void PurgeChunk(string name, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PurgeChunk(name, useModel, model));
        }
        
        public void DefineModule(string name, List<dynamic> buffers,List<dynamic> parameters,string version, string doc, string inter, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new DefineModule(name, buffers, parameters,version,doc,inter));
        }
        
        public void CompleteRequest(string specId, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new CompleteRequest(specId, useModel, model));
        }
        
        public void CompleteAllBufferRequests(string bufferName, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllBufferRequests(bufferName, useModel, model));
        }
        
        public void CompleteAllModuleRequests(string moduleName, bool useModel = false,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllModuleRequests(moduleName, useModel, model));
        }
        
        public void CommandOutput(string command, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new CommandOutput(command, useModel, model));
        }
        
        public void ChunkCopiedFrom(string chunkName, bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new ChunkCopiedFrom(chunkName, useModel, model));
        }
        
        public void MpTime(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new MpTime(useModel, model));
        }
        
        public void MpTimeMs(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new MpTimeMs(useModel, model));
        }
        
        public void PrintBoldResponseData(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new PrintBoldResponseData(useModel, model));
        }
        
        public void Pbreak(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Pbreak(useModel, model));
        }
        
        public void Punbreak(bool useModel = false, string model = null)
        {
            SendEvaluationRequest(new Punbreak(useModel, model));
        }
    }
}