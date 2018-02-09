using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintAudicon : AbstractEvaluationRequest
    {
        public PrintAudicon(string model = null) : base("print-audicon", model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}