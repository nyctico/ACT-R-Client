using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetParameterValue : AbstractEvaluationRequest
    {
        public SetParameterValue(string parameterName, dynamic newValue, string model = null) : base(
            "set-parameter-value",
            model)
        {
            ParameterName = parameterName;
            NewValue = newValue;
        }

        public string ParameterName { get; set; }
        public dynamic NewValue { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ParameterName);
            list.Add(NewValue);

            return list;
        }
    }
}