namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AllProductions : AbstractEvaluationRequest
    {
        public AllProductions(string model = null) : base("all-productions", model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}