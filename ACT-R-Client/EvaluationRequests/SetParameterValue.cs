namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetParameterValue : AbstractEvaluationRequest
    {
        public SetParameterValue(string parameterName, object newValue, string model = null) : base(
            "set-parameter-value",
            model)
        {
            ParameterName = parameterName;
            NewValue = newValue;
        }

        public string ParameterName { get; set; }
        public object NewValue { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(ParameterName);
            list.Add(NewValue);

            return list.ToArray();
        }
    }
}