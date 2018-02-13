using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(List<object> requestDetails, string model = null) : base(
            "simulate-retrieval-request",
            model)
        {
            RequestDetails = requestDetails;
        }

        public List<object> RequestDetails { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(RequestDetails);

            return list;
        }
    }
}