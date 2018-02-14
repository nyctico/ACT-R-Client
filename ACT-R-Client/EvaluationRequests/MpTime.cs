namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTime : AbstractEvaluationRequest
    {
        public MpTime(string model = null) : base("mp-time", model)
        {
        }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}