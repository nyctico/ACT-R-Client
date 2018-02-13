using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Spp : AbstractEvaluationRequest
    {
        public Spp(List<object> parameters, string model = null) : base("spp",
            model)
        {
            Parameters = parameters;
        }

        public List<object> Parameters { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}