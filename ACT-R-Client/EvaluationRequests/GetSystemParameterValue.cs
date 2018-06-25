using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetSystemParameterValue : AbstractEvaluationRequest
    {
        public GetSystemParameterValue(string systemParameterName, string model = null) : base(
            "get-system-parameter-value",
            model)
        {
            SystemParameterName = systemParameterName;
        }

        public string SystemParameterName { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(SystemParameterName);
        }
    }
}