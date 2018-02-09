using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class WhynotDm : AbstractEvaluationRequest
    {
        public WhynotDm(List<dynamic> parameters, string model = null) : base("whynot-dm",
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