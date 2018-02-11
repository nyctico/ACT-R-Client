using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Penable : AbstractEvaluationRequest
    {
        public Penable(List<string> productionNames, string model = null) : base("penable",
            model)
        {
            ProductionNames = productionNames;
        }

        public List<string> ProductionNames { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ProductionNames);

            return list;
        }
    }
}