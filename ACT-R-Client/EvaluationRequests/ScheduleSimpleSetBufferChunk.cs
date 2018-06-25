using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleSetBufferChunk : AbstractEvaluationRequest
    {
        public ScheduleSimpleSetBufferChunk(string buffer, string chunk, int time,
            string module = "NONE", int priority = 0, bool requested = true,
            string model = null) : base("schedule-simple-set-buffer-chunk", model)
        {
            Buffer = buffer;
            Chunk = chunk;
            Time = time;
            Module = module;
            Priority = priority;
            Requested = requested;
        }

        public string Buffer { set; get; }
        public string Chunk { set; get; }
        public int Time { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }
        public bool Requested { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Buffer);
            parameterList.Add(Chunk);
            parameterList.Add(Time);
            parameterList.Add(Module);
            parameterList.Add(Priority);
            parameterList.Add(Requested);
        }
    }
}