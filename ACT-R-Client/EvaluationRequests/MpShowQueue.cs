using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpShowQueue : AbstractEvaluationRequest
    {
        public MpShowQueue(bool indicateTraced = false, bool useModel = false, string model = null) : base("mp-show-queue", useModel,
            model)
        {
            IndicateTraced = indicateTraced;
        }

        public bool IndicateTraced { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(IndicateTraced);

            return list;
        }
    }
}