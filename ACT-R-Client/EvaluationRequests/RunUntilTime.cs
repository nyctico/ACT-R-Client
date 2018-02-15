namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunUntilTime : AbstractEvaluationRequest
    {
        public RunUntilTime(int time, bool realTime = false, string model = null) : base(
            "run-until-time", model)
        {
            Time = time;
            RealTime = realTime;
        }

        public int Time { set; get; }
        public bool RealTime { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(RealTime);

            return list.ToArray();
        }
    }
}