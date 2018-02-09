using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Punbreak : AbstractEvaluationRequest
    {
        public Punbreak(string model = null) : base("punbreak", model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}