using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpModels : AbstractEvaluationRequest
    {
        public MpModels(string model = null) : base("mp-models", model)
        {
        }

        public override void AddParameterToList(List<object> parameterList){}
    }
}