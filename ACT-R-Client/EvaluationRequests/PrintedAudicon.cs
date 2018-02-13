using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedAudicon : AbstractEvaluationRequest
    {
        public PrintedAudicon(string model = null) : base("printed-audicon", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}