using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintVisicon: AbstractEvaluationRequest
    {

        public PrintVisicon(bool useModel = false, string model = null) : base("print-visicon", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}