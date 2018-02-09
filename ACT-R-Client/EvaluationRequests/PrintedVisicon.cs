using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedVisicon : AbstractEvaluationRequest
    {
        public PrintedVisicon(string model = null) : base("printed-visicon", model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}