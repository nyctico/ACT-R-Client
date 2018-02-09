using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    /// <summary>
    ///     Client for interaction with an ACT-R Dispatcher
    /// </summary>
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

        /// <summary>
        ///     Reset the ACT-R scheduler and all models.
        /// </summary>
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
        public List<dynamic> PermuteList(List<dynamic> list)
        {
            return SendEvaluationRequest(new PermuteList(list)).ReturnValue.ToObject<List<dynamic>>();
        }

        /// <summary>
        ///     Create a window for a task.
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="isVisible">Indicates, if the window should be visible</param>
        /// <param name="width">Window withd</param>
        /// <param name="height">Window height</param>
        /// <param name="x">Window X-Coordinate</param>
        /// <param name="y">Window Y-Coordinate</param>
        /// <param name="useModel">Indicates if a special model should be used</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Window OpenExpWindow(string title, bool isVisible, int width = 300, int height = 300, int x = 300,
            int y = 300,
            string model = null)
        {
            List<dynamic> returnValue =
                SendEvaluationRequest(new OpenExpWindow(title, isVisible, width, height, x, y, model))
                    .ReturnValue.ToObject<List<dynamic>>();
            return new Window(returnValue[0], returnValue[1], returnValue[2]);
        }

        public void AddTextToWindow(Window window, string text, int x, int y, string color = "black", int height = 50,
            int width = 75,
            int fontSize = 12, string model = null)
        {
            SendEvaluationRequest(new AddTextToWindow(window, text, x, y, color, height, width, fontSize,
                model));
        }

        public void AddButtonToExpWindow(Window window, string text, int x, int y, List<dynamic> action = null,
            int height = 50,
            int width = 75,
            string color = "gray", string model = null)
        {
            SendEvaluationRequest(new AddButtonToExpWindow(window, text, x, y, action, height, width, color,
                model));
        }

        public Line AddLineToExpWindow(Window window, int[] start, int[] end, string color = null,
            string model = null)
        {
            var returnValue = SendEvaluationRequest(new AddLineToExpWindow(window, start, end, color,
                model)).ReturnValue.ToObject<List<dynamic>>();
            return new Line(returnValue[0], returnValue[1]);
        }

        public void ModifyLineForExpWindow(Line line, int[] start, int[] end, string color = null,
            string model = null)
        {
            SendEvaluationRequest(new ModifyLineForExpWindow(line, start, end, color,
                model));
        }

        public void RemoveItemFromExpWindow(Window window, IItem item,
            string model = null)
        {
            var removeItemsFromExpWindow = new RemoveItemFromExpWindow(window, item, model);
            SendEvaluationRequest(removeItemsFromExpWindow);
        }

        public void ClearExpWindow(Window window, string model = null)
        {
            SendEvaluationRequest(new ClearExpWindow(window, model));
        }

        public double Correlation(List<dynamic> results, List<dynamic> data, bool output = true,
            string model = null)
        {
            return SendEvaluationRequest(new Correlation(results, data, output, model)).ReturnValue;
        }

        public double MeanDeviation(List<dynamic> results, List<dynamic> data, bool output = true,
            string model = null)
        {
            return SendEvaluationRequest(new MeanDeviation(results, data, output, model)).ReturnValue;
        }

        public long ActrRandom(long value, string model = null)
        {
            return SendEvaluationRequest(new ActrRandom(value, model)).ReturnValue;
        }

        public void InstallDevice(Window window, string model = null)
        {
            SendEvaluationRequest(new InstallDevice(window, model));
        }

        public void Run(int time, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new Run(time, realTime, model));
        }

        public void NewToneSound(int frequence, double duration, double? onset = null, bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewToneSound(frequence, duration, onset, timeInMs, model));
        }

        public void NewWordSound(string word, double? onset = null, string location = "external", bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewWordSound(word, onset, location, timeInMs, model));
        }

        public void NewDigitSound(long digit, double? onset = null, bool timeInMs = false,
            string model = null)
        {
            SendEvaluationRequest(new NewDigitSound(digit, onset, timeInMs, model));
        }

        public void ScheduleSimpleEventRelative(long timeDelay, string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventRelative(timeDelay, action, parameters, module, priority,
                maintenance, model));
        }

        public void ScheduleSimpleEventNow(string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventNow(action, parameters, module, priority,
                maintenance, model));
        }

        public void ScheduleSimpleEvent(long time, string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEvent(time, action, parameters, module, priority,
                maintenance, model));
        }

        public void Reload(bool compile = false, string model = null)
        {
            SendEvaluationRequest(new Reload(compile));
        }

        public void RunFullTime(int time, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunFullTime(time, realTime, model));
        }

        public void RunUntilTime(int time, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunUntilTime(time, realTime, model));
        }

        public void RunNEvents(long eventCount, bool realTime = false, string model = null)
        {
            SendEvaluationRequest(new RunNEvents(eventCount, realTime, model));
        }

        public void RunUntilCondition(string condition, bool realTime = false,
            string model = null)
        {
            SendEvaluationRequest(new RunUntilCondition(condition, realTime, model));
        }

        public void BufferChunk(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new BufferChunk(parameters, model));
        }

        public List<dynamic> BufferStatus(List<dynamic> parameters, string model = null)
        {
            return SendEvaluationRequest(new BufferStatus(parameters, model)).ReturnValue;
        }

        public List<dynamic> BufferRead(string buffer, string model = null)
        {
            return SendEvaluationRequest(new BufferRead(buffer, model)).ReturnValue;
        }

        public void ClearBuffer(string buffer, string model = null)
        {
            SendEvaluationRequest(new ClearBuffer(buffer, model));
        }

        public void Whynot(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Whynot(parameters, model));
        }

        public void WhynotDm(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new WhynotDm(parameters, model));
        }

        public void Penable(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Penable(parameters, model));
        }

        public void Pdisable(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Pdisable(parameters, model));
        }

        public void GoalFocus(string goal = null, string model = null)
        {
            SendEvaluationRequest(new GoalFocus(goal, model));
        }

        public void PrintWarning(string warning, string model = null)
        {
            SendEvaluationRequest(new PrintWarning(warning, model));
        }

        public void ActrOutput(string output, string model = null)
        {
            SendEvaluationRequest(new ActrOutput(output, model));
        }

        public void PrintVisicon(string model = null)
        {
            SendEvaluationRequest(new PrintVisicon(model));
        }

        public void GetTime(bool modelTime = true, string model = null)
        {
            SendEvaluationRequest(new GetTime(modelTime, model));
        }

        public void DefineChunks(List<dynamic> chunks, string model = null)
        {
            SendEvaluationRequest(new DefineChunks(chunks, model));
        }

        public void AddDm(List<dynamic> chunks, string model = null)
        {
            SendEvaluationRequest(new AddDm(chunks, model));
        }

        public void PprintChunks(List<dynamic> chunks, string model = null)
        {
            SendEvaluationRequest(new PprintChunks(chunks, model));
        }

        public void ChunkSlotValue(string chunkName, string slotName, string model = null)
        {
            SendEvaluationRequest(new ChunkSlotValue(chunkName, slotName, model));
        }

        public void SetChunkSlotValue(string chunkName, string slotName, string newValue,
            string model = null)
        {
            SendEvaluationRequest(new SetChunkSlotValue(chunkName, slotName, newValue, model));
        }

        public void ModChunk(string chunkName, List<dynamic> mods, string model = null)
        {
            SendEvaluationRequest(new ModChunk(chunkName, mods, model));
        }

        public void ModFocus(List<dynamic> mods, string model = null)
        {
            SendEvaluationRequest(new ModFocus(mods, model));
        }

        public void ChunkP(string chunkName, string model = null)
        {
            SendEvaluationRequest(new ChunkP(chunkName, model));
        }

        public void CopyChunk(string chunkName, string model = null)
        {
            SendEvaluationRequest(new CopyChunk(chunkName, model));
        }

        public void ExtendPossibleSlots(string chunkName, bool warn = true, string model = null)
        {
            SendEvaluationRequest(new ExtendPossibleSlots(chunkName, warn, model));
        }

        public void ModelOutput(string output, string model = null)
        {
            SendEvaluationRequest(new ModelOutput(output, model));
        }

        public void SetBufferChunk(string bufferName, string chunkName, bool requested = true,
            string model = null)
        {
            SendEvaluationRequest(new SetBufferChunk(bufferName, chunkName, requested, model));
        }

        public void StartHandAtMouse(string model = null)
        {
            SendEvaluationRequest(new StartHandAtMouse(model));
        }

        public void MpShowQueue(bool indicateTraced, string model = null)
        {
            SendEvaluationRequest(new MpShowQueue(indicateTraced, model));
        }

        public void PrintDmFinsts(string model = null)
        {
            SendEvaluationRequest(new PrintDmFinsts(model));
        }

        public void Spp(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Spp(parameters, model));
        }

        public void MpModels(string model = null)
        {
            SendEvaluationRequest(new MpModels(model));
        }

        public void AllProductions(string model = null)
        {
            SendEvaluationRequest(new AllProductions(model));
        }

        public void Buffers(string model = null)
        {
            SendEvaluationRequest(new Buffers(model));
        }

        public void PrintedVisicon(string model = null)
        {
            SendEvaluationRequest(new PrintedVisicon(model));
        }

        public void PrintAudicon(string model = null)
        {
            SendEvaluationRequest(new PrintAudicon(model));
        }

        public void PrintedAudicon(string model = null)
        {
            SendEvaluationRequest(new PrintedAudicon(model));
        }

        public void PrintedParameterDetails(string parameter, string model = null)
        {
            SendEvaluationRequest(new PrintedParameterDetails(parameter, model));
        }

        public void SortedModuleNames(string model = null)
        {
            SendEvaluationRequest(new SortedModuleNames(model));
        }

        public void ModulesParameters(string module, string model = null)
        {
            SendEvaluationRequest(new ModulesParameters(module, model));
        }

        public void ModulesWithParameters(string model = null)
        {
            SendEvaluationRequest(new ModulesWithParameters(model));
        }

        public void UsedProductionBuffers(string model = null)
        {
            SendEvaluationRequest(new UsedProductionBuffers(model));
        }

        public void RecordHistory(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new RecordHistory(parameters, model));
        }

        public void StopRecordingHistory(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new StopRecordingHistory(parameters, model));
        }

        public void GetHistoryData(string history, List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new GetHistoryData(history, parameters, model));
        }

        public void ProcessHistoryData(string processor, bool file, List<dynamic> dataParameters,
            List<dynamic> processorParameters, string model = null)
        {
            SendEvaluationRequest(new ProcessHistoryData(processor, file, dataParameters, processorParameters,
                model));
        }

        public void SaveHistoryData(string history, bool file, string comments, List<dynamic> parameters,
            string model = null)
        {
            SendEvaluationRequest(new SaveHistoryData(history, file, comments, parameters, model));
        }

        public void Dm(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Dm(parameters, model));
        }

        public void Sdm(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Sdm(parameters, model));
        }

        public void GetParameterValue(string parameter, string model = null)
        {
            SendEvaluationRequest(new GetParameterValue(parameter, model));
        }

        public void SetParameterValue(string parameter, string value, string model = null)
        {
            SendEvaluationRequest(new SetParameterValue(parameter, value, model));
        }

        public void GetSystemParameterValue(string parameter, string model = null)
        {
            SendEvaluationRequest(new GetSystemParameterValue(parameter, model));
        }

        public void SetSystemParameterValue(string parameter, string value, string model = null)
        {
            SendEvaluationRequest(new SetSystemParameterValue(parameter, value, model));
        }

        public void Sdp(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Sdp(parameters, model));
        }

        public void SimulateRetrievalRequest(List<dynamic> spec, string model = null)
        {
            SendEvaluationRequest(new SimulateRetrievalRequest(spec, model));
        }

        public void SavedActivationHistory(string model = null)
        {
            SendEvaluationRequest(new SavedActivationHistory(model));
        }

        public void PrintActivationTrace(int time, bool ms = true, string model = null)
        {
            SendEvaluationRequest(new PrintActivationTrace(time, ms, model));
        }

        public void PrintChunkActivationTrace(string chunkName, int time, bool ms = true,
            string model = null)
        {
            SendEvaluationRequest(new PrintChunkActivationTrace(chunkName, time, ms, model));
        }

        public void Pp(List<dynamic> parameters, string model = null)
        {
            SendEvaluationRequest(new Pp(parameters, model));
        }

        public void TriggerReward(string reward, bool maintenance = false, string model = null)
        {
            SendEvaluationRequest(new TriggerReward(reward, maintenance, model));
        }

        public void DefineChunkSpec(List<dynamic> spec, string model = null)
        {
            SendEvaluationRequest(new DefineChunkSpec(spec, model));
        }

        public void ChunkSpecToChunkDef(string specId, string model = null)
        {
            SendEvaluationRequest(new ChunkSpecToChunkDef(specId, model));
        }

        public void ReleaseChunkSpecId(string specId, string model = null)
        {
            SendEvaluationRequest(new ReleaseChunkSpecId(specId, model));
        }

        public void ScheduleSimpleEventAfterModule(string afterModule, string action, List<dynamic> parameters = null,
            string module = "NONE", bool maintenance = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleEventAfterModule(afterModule, action, parameters, module,
                maintenance, model));
        }

        public void ScheduleSimpleSetBufferChunk(string buffer, string chunk, int time,
            string module = "NONE", int priority = 0, bool requested = false,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleSetBufferChunk(buffer, chunk, time, module, priority,
                requested, model));
        }

        public void ScheduleSimpleModBufferChunk(string buffer, List<dynamic> modListOrSpec, int time,
            string module = "NONE", int priority = 0,
            string model = null)
        {
            SendEvaluationRequest(new ScheduleSimpleModBufferChunk(buffer, modListOrSpec, time, module, priority,
                model));
        }

        public void UndefineModule(string name, string model = null)
        {
            SendEvaluationRequest(new UndefineModule(name, model));
        }

        public void DeleteChunk(string name, string model = null)
        {
            SendEvaluationRequest(new DeleteChunk(name, model));
        }

        public void PurgeChunk(string name, string model = null)
        {
            SendEvaluationRequest(new PurgeChunk(name, model));
        }

        public void DefineModule(string name, List<dynamic> buffers, List<dynamic> parameters, string version,
            string doc, string inter, string model = null)
        {
            SendEvaluationRequest(new DefineModule(name, buffers, parameters, version, doc, inter));
        }

        public void CompleteRequest(string specId, string model = null)
        {
            SendEvaluationRequest(new CompleteRequest(specId, model));
        }

        public void CompleteAllBufferRequests(string bufferName,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllBufferRequests(bufferName, model));
        }

        public void CompleteAllModuleRequests(string moduleName,
            string model = null)
        {
            SendEvaluationRequest(new CompleteAllModuleRequests(moduleName, model));
        }

        public void CommandOutput(string command, string model = null)
        {
            SendEvaluationRequest(new CommandOutput(command, model));
        }

        public void ChunkCopiedFrom(string chunkName, string model = null)
        {
            SendEvaluationRequest(new ChunkCopiedFrom(chunkName, model));
        }

        public void MpTime(string model = null)
        {
            SendEvaluationRequest(new MpTime(model));
        }

        public void MpTimeMs(string model = null)
        {
            SendEvaluationRequest(new MpTimeMs(model));
        }

        public void PrintBoldResponseData(string model = null)
        {
            SendEvaluationRequest(new PrintBoldResponseData(model));
        }

        public void Pbreak(string model = null)
        {
            SendEvaluationRequest(new Pbreak(model));
        }

        public void Punbreak(string model = null)
        {
            SendEvaluationRequest(new Punbreak(model));
        }
    }
}