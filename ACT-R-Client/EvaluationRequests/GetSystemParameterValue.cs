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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(SystemParameterName);

            return list.ToArray();
        }
    }
}