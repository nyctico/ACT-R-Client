using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTimeMs : AbstractEvaluationRequest
    {
        public MpTimeMs(string model = null) : base("mp-time-ms", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}