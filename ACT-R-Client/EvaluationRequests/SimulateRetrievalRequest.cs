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

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(RequestDetails);

            return list.ToArray();
        }
    }
}