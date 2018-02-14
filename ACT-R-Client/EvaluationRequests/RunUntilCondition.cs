namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunUntilCondition : AbstractEvaluationRequest
    {
        public RunUntilCondition(string condition, bool realTime = false, string model = null) :
            base("run-until-condition", model)
        {
            Condition = condition;
            RealTime = realTime;
        }

        public string Condition { set; get; }
        public bool RealTime { set; get; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Condition);
            list.Add(RealTime);

            return list.ToArray();
        }
    }
}