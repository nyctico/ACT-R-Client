using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Whynot : AbstractEvaluationRequest
    {
        public Whynot(List<string> productionNames, string model = null) : base("whynot",
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