using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintVisicon : AbstractEvaluationRequest
    {
        public PrintVisicon(string model = null) : base("print-visicon", model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}