using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SavedActivationHistory : AbstractEvaluationRequest
    {
        public SavedActivationHistory(bool useModel = false, string model = null) : base("saved-activation-history",
            useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}