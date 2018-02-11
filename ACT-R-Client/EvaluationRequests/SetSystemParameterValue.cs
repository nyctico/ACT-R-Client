using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetSystemParameterValue : AbstractEvaluationRequest
    {
        public SetSystemParameterValue(string systemParameterName, dynamic newValue, string model = null) :
            base("set-system-parameter-value",
                model)
        {
            SystemParameterName = systemParameterName;
            NewValue = newValue;
        }

        public string SystemParameterName { get; set; }
        public dynamic NewValue { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(SystemParameterName);
            list.Add(NewValue);

            return list;
        }
    }
}