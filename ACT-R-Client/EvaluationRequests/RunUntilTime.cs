using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunUntilTime : AbstractEvaluationRequest
    {
        public RunUntilTime(int time, bool realTime = false, string model = null) : base(
            "run-until-time", model)
        {
            Time = time;
            RealTime = realTime;
        }

        public int Time { set; get; }
        public bool RealTime { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Time);
            parameterList.Add(RealTime);
        }
    }
}