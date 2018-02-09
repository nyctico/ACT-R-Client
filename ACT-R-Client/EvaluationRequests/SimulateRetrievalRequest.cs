using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(List<dynamic> spec, bool useModel = false, string model = null) : base(
            "simulate-retrieval-request", useModel,
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