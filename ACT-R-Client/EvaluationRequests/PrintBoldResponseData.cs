using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintBoldResponseData: AbstractEvaluationRequest
    {
        public PrintBoldResponseData(bool useModel = false, string model = null) : base("print-bold-response-data", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}