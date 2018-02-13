using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetParameterValue : AbstractEvaluationRequest
    {
        public GetParameterValue(string parameterName, string model = null) : base(
            "get-parameter-value",
            model)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ParameterName);

            return list;
        }
    }
}