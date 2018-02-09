using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Penable : AbstractEvaluationRequest
    {
        public Penable(List<dynamic> parameters, string model = null) : base("penable",
            model)
        {
            Parameters = parameters;
        }

        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}