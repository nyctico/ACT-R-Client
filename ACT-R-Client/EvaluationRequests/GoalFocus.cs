namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GoalFocus : AbstractEvaluationRequest
    {
        public GoalFocus(string chunkName, string model = null) : base("goal-focus",
            model)
        {
            ChunkName = chunkName;
        }

        public string ChunkName { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list.ToArray();
        }
    }
}