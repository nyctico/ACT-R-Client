using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Punbreak : AbstractEvaluationRequest
    {
        public Punbreak(bool useModel = false, string model = null) : base("punbreak", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}