using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdp: AbstractEvaluationRequest
    {
        public Sdp(List<dynamic> parameters, bool useModel = false, string model = null) : base("sdp", useModel,
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