using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SavedActivationHistory : AbstractEvaluationRequest
    {
        public SavedActivationHistory(string model = null) : base("saved-activation-history",
            model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}