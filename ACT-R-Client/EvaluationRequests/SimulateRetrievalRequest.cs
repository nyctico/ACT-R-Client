using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SimulateRetrievalRequest : AbstractEvaluationRequest
    {
        public SimulateRetrievalRequest(object[] requestDetails, string model = null) : base(
            "simulate-retrieval-request",
            model)
        {
            RequestDetails = requestDetails;
        }

        public object[] RequestDetails { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(RequestDetails);
        }
    }
}