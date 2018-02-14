namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintDmFinsts : AbstractEvaluationRequest
    {
        public PrintDmFinsts(string model = null) : base("print-dm-finsts", model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}