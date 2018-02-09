using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SortedModuleNames : AbstractEvaluationRequest
    {
        public SortedModuleNames(bool useModel = false, string model = null) : base("sorted-module-names", useModel,
            model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}