namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleModBufferChunk : AbstractEvaluationRequest
    {
        public ScheduleSimpleModBufferChunk(string buffer, object[] modListOrSpec, int time,
            string module = "NONE", int priority = 0,
            string model = null) : base("schedule-simple-mod-buffer-chunk", model)
        {
            Buffer = buffer;
            ModListOrSpec = modListOrSpec;
            Time = time;
            Module = module;
            Priority = priority;
        }

        public string Buffer { set; get; }
        public object[] ModListOrSpec { set; get; }
        public int Time { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Buffer);
            list.Add(ModListOrSpec);
            list.Add(Time);
            list.Add(Module);
            list.Add(Priority);

            return list.ToArray();
        }
    }
}