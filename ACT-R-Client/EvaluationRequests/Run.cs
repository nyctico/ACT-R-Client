using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Run : AbstractEvaluationRequest
    {
        public Run(int time, bool realTime = false, string model = null) : base("run",
            model)
        {
            Time = time;
            RealTime = realTime;
        }

        public int Time { set; get; }
        public bool RealTime { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Time);
            parameterList.Add(RealTime);
        }
    }
}