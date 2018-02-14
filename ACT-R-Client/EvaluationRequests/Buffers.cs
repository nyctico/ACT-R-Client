namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Buffers : AbstractEvaluationRequest
    {
        public Buffers(string model = null) : base("buffers", model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}