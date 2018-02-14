using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Pdisable : AbstractEvaluationRequest
    {
        public Pdisable(List<string> productionNames, string model = null) : base("pdisable",
            model)
        {
            ProductionNames = productionNames;
        }

        public List<string> ProductionNames { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ProductionNames);

            return list.ToArray();
        }
    }
}