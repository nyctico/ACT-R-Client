using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintVisicon : AbstractEvaluationRequest
    {
        public PrintVisicon(string model = null) : base("print-visicon", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}