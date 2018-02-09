using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetSystemParameterValue : AbstractEvaluationRequest
    {
        public SetSystemParameterValue(string parameter, string value, string model = null) :
            base("set-system-parameter-value",
                model)
        {
            Parameter = parameter;
            Value = value;
        }

        public string Parameter { get; set; }
        public string Value { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameter);
            list.Add(Value);

            return list;
        }
    }
}