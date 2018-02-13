using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SavedActivationHistory : AbstractEvaluationRequest
    {
        public SavedActivationHistory(string model = null) : base("saved-activation-history",
            model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}