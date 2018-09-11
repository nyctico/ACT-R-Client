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

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ProductionNames);
        }
    }
}