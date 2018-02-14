namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdp : AbstractEvaluationRequest
    {
        public Sdp(object[] parameters, string model = null) : base("sdp",
            model)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list.ToArray();
        }
    }
}