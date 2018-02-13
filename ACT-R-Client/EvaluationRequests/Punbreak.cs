using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Punbreak : AbstractEvaluationRequest
    {
        public Punbreak(List<string> productionNames, string model = null) : base("punbreak", model)
        {
            ProductionNames = productionNames;
        }

        public List<string> ProductionNames { set; get; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ProductionNames);

            return list;
        }
    }
}