using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Buffers : AbstractEvaluationRequest
    {
        public Buffers(string model = null) : base("buffers", model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}