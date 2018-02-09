using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleModBufferChunk : AbstractEvaluationRequest
    {
        public ScheduleSimpleModBufferChunk(string buffer, List<dynamic> modListOrSpec, int time,
            string module = "NONE", int priority = 0, bool useModel = false,
            string model = null) : base("schedule-simple-mod-buffer-chunk", useModel, model)
        {
            Buffer = buffer;
            ModListOrSpec = modListOrSpec;
            Time = time;
            Module = module;
            Priority = priority;
        }

        public string Buffer { set; get; }
        public List<dynamic> ModListOrSpec { set; get; }
        public int Time { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);
            list.Add(ModListOrSpec);
            list.Add(Time);
            list.Add(Module);
            list.Add(Priority);

            return list;
        }
    }
}