namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class StartHandAtMouse : AbstractEvaluationRequest
    {
        public StartHandAtMouse(string model = null) : base("start-hand-at-mouse",
            model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}