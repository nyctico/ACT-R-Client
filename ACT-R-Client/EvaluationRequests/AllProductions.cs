using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AllProductions : AbstractEvaluationRequest
    {
        public AllProductions(bool useModel = false, string model = null) : base("all-productions", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}