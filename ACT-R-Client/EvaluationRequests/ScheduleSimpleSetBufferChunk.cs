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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);
            list.Add(Chunk);
            list.Add(Time);
            list.Add(Module);
            list.Add(Priority);
            list.Add(Requested);

            return list.ToArray();
        }
    }
}