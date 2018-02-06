using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class GoalFocus: AbstractDispatcherEvaluate
    {
        public string Goal { get; set; }

        public GoalFocus(string goal=null, bool useModel = false, string model = null) : base("goal-focus", useModel, model)
        {
            Goal = goal;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Goal);

            return list;
        }
    }
}