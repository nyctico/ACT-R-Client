using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedParameterDetails : AbstractEvaluationRequest
    {
        public PrintedParameterDetails(string parameterName, string model = null) : base(
            "printed-parameter-details",
            model)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ParameterName);
        }
    }
}