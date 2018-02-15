namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdm : AbstractEvaluationRequest
    {
        public Sdm(object[] specifications, string model = null) : base("sdm",
            model)
        {
            Specifications = specifications;
        }

        public object[] Specifications { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Specifications);

            return list.ToArray();
        }
    }
}