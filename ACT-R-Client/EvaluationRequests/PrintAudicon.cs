namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintAudicon : AbstractEvaluationRequest
    {
        public PrintAudicon(string model = null) : base("print-audicon", model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}