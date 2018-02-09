using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GoalFocus : AbstractEvaluationRequest
    {
        public GoalFocus(string goal = null, string model = null) : base("goal-focus",
            model)
        {
            Goal = goal;
        }

        public string Goal { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Goal);

            return list;
        }
    }
}