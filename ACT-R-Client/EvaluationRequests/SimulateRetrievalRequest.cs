using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(dynamic[] requestDetails, string model = null) : base(
            "simulate-retrieval-request",
            model)
        {
            RequestDetails = requestDetails;
        }

        public dynamic[] RequestDetails { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(RequestDetails);
        }
    }
}