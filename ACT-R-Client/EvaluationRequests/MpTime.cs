using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTime : AbstractEvaluationRequest
    {
        public MpTime(bool useModel = false, string model = null) : base("mp-time", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}