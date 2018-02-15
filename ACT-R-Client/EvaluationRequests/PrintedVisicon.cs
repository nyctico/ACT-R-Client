namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedVisicon : AbstractEvaluationRequest
    {
        public PrintedVisicon(string model = null) : base("printed-visicon", model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}