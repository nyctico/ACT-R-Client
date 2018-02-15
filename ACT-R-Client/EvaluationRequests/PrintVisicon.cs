namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintVisicon : AbstractEvaluationRequest
    {
        public PrintVisicon(string model = null) : base("print-visicon", model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}