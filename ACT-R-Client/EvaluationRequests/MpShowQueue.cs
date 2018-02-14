namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpShowQueue : AbstractEvaluationRequest
    {
        public MpShowQueue(bool indicateTraced = false, string model = null) : base(
            "mp-show-queue",
            model)
        {
            IndicateTraced = indicateTraced;
        }

        public bool IndicateTraced { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(IndicateTraced);

            return list.ToArray();
        }
    }
}