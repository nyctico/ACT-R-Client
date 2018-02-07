using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunNEvents: AbstractEvaluationRequest
    {
        public long EventCount { set; get; }
        public bool RealTime { set; get; }

        public RunNEvents(long eventCount, bool realTime=false, bool useModel = false, string model = null) : base("run-n-events", useModel, model)
        {
            EventCount = eventCount;
            RealTime = realTime;
        }
        
        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(EventCount);
            list.Add(RealTime);

            return list;
        }
    }
}