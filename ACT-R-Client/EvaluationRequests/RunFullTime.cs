using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunFullTime : AbstractEvaluationRequest
    {
        public int Time { set; get; }
        public bool RealTime { set; get; }

        public RunFullTime(int time, bool realTime=false, bool useModel = false, string model = null) : base("run-full-time", useModel, model)
        {
            Time = time;
            RealTime = realTime;
        }
        
        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(RealTime);

            return list;
        }
    }
}