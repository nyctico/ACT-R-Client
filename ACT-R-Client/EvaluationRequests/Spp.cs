namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Spp : AbstractEvaluationRequest
    {
        public Spp(object[] parameters, string model = null) : base("spp",
            model)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list.ToArray();
        }
    }
}