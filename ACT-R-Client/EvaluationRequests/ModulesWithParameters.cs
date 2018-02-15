namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesWithParameters : AbstractEvaluationRequest
    {
        public ModulesWithParameters(string model = null) : base("modules-with-parameters",
            model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}