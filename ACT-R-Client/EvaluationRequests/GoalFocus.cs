using System.Collections.Generic;

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

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ChunkName);
        }
    }
}