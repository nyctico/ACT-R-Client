using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetSystemParameterValue : AbstractEvaluationRequest
    {
        public SetSystemParameterValue(string systemParameterName, object newValue, string model = null) :
            base("set-system-parameter-value",
                model)
        {
            SystemParameterName = systemParameterName;
            NewValue = newValue;
        }

        public string SystemParameterName { get; set; }
        public object NewValue { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(SystemParameterName);
            list.Add(NewValue);

            return list;
        }
    }
}