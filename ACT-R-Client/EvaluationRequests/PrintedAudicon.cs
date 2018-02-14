namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedAudicon : AbstractEvaluationRequest
    {
        public PrintedAudicon(string model = null) : base("printed-audicon", model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}