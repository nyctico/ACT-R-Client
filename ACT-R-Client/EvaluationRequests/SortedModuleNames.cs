namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SortedModuleNames : AbstractEvaluationRequest
    {
        public SortedModuleNames(string model = null) : base("sorted-module-names",
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