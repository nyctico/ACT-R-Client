using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetSystemParameterValue: AbstractEvaluationRequest
    {
        public GetSystemParameterValue(string parameter, bool useModel = false, string model = null) : base("get-system-parameter-value", useModel,
            model)
        {
            Parameter = parameter;
        }

        public string Parameter { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameter);

            return list;
        }
    }
}