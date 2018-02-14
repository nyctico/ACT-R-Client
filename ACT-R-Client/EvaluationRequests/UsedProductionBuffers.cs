namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class UsedProductionBuffers : AbstractEvaluationRequest
    {
        public UsedProductionBuffers(string model = null) : base("used-production-buffers",
            model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}