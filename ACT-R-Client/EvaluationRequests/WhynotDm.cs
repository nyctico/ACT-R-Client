using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class WhynotDm: AbstractEvaluationRequest
    {
        public List<dynamic> Parameters { get; set; }

        public WhynotDm(List<dynamic> parameters, bool useModel = false, string model = null) : base("whynot-dm", useModel, model)
        {
            Parameters = parameters;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}