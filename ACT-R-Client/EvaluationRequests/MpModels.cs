using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpModels: AbstractEvaluationRequest
    {
        public MpModels(bool useModel = false, string model = null) : base("mp-models", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}