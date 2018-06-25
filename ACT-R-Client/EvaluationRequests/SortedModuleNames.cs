using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SortedModuleNames : AbstractEvaluationRequest
    {
        public SortedModuleNames(string model = null) : base("sorted-module-names",
            model)
        {
        }

        public override void AddParameterToList(List<object> parameterList)
        {
        }
    }
}