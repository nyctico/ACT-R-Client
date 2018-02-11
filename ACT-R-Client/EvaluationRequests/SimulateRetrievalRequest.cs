using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(List<dynamic> requestDetails, string model = null) : base(
            "simulate-retrieval-request",
            model)
        {
            RequestDetails = requestDetails;
        }

        public List<dynamic> RequestDetails { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(RequestDetails);

            return list;
        }
    }
}