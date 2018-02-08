using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class StartHandAtMouse: AbstractEvaluationRequest
    {
        public StartHandAtMouse(bool useModel = false, string model = null) : base("start-hand-at-mouse", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}