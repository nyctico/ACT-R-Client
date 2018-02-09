using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedVisicon: AbstractEvaluationRequest
    {
        public PrintedVisicon(bool useModel = false, string model = null) : base("printed-visicon", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}