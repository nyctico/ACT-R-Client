namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTimeMs : AbstractEvaluationRequest
    {
        public MpTimeMs(string model = null) : base("mp-time-ms", model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}