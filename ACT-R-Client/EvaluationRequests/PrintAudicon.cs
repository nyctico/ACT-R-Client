using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintAudicon : AbstractEvaluationRequest
    {
        public PrintAudicon(bool useModel = false, string model = null) : base("print-audicon", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}