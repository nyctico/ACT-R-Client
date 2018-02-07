using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunUntilTime : AbstractEvaluationRequest
    {
        public RunUntilTime(int time, bool realTime = false, bool useModel = false, string model = null) : base(
            "run-until-time", useModel, model)
        {
            Time = time;
            RealTime = realTime;
        }

        public int Time { set; get; }
        public bool RealTime { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(RealTime);

            return list;
        }
    }
}