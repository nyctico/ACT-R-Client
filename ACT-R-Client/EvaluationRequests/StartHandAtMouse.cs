using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class StartHandAtMouse : AbstractEvaluationRequest
    {
        public StartHandAtMouse(string model = null) : base("start-hand-at-mouse",
            model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}