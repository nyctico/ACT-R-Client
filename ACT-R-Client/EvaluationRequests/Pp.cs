using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Pp : AbstractEvaluationRequest
    {
        public Pp(List<string> parameters, string model = null) : base("pp",
            model)
        {
            Parameters = parameters;
        }

        public List<string> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}