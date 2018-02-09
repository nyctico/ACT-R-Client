using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTimeMs : AbstractEvaluationRequest
    {
        public MpTimeMs(bool useModel = false, string model = null) : base("mp-time-ms", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}