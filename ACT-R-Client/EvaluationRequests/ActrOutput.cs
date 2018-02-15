namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ActrOutput : AbstractEvaluationRequest
    {
        public ActrOutput(string output, string model = null) : base("act-r-output",
            model)
        {
            Output = output;
        }

        public string Output { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Output);

            return list.ToArray();
        }
    }
}