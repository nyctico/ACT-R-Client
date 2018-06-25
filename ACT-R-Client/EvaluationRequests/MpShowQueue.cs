using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpShowQueue : AbstractEvaluationRequest
    {
        public MpShowQueue(bool indicateTraced = false, string model = null) : base(
            "mp-show-queue",
            model)
        {
            IndicateTraced = indicateTraced;
        }

        public bool IndicateTraced { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(IndicateTraced);
        }
    }
}