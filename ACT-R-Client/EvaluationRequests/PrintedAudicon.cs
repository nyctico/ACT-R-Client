using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedAudicon : AbstractEvaluationRequest
    {
        public PrintedAudicon(string model = null) : base("printed-audicon", model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}