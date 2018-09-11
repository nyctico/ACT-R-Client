using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunFullTime : AbstractEvaluationRequest
    {
        public RunFullTime(int time, bool realTime = false, string model = null) : base(
            "run-full-time", model)
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