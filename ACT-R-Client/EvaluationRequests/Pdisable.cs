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

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ProductionNames);
        }
    }
}