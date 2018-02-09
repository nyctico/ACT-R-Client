using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(List<dynamic> spec, string model = null) : base(
            "simulate-retrieval-request",
            model)
        {
            Spec = spec;
        }

        public List<dynamic> Spec { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Spec);

            return list;
        }
    }
}