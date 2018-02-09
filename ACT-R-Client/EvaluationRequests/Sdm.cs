using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdm : AbstractEvaluationRequest
    {
        public Sdm(List<dynamic> parameters, bool useModel = false, string model = null) : base("sdm", useModel,
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