using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Pbreak : AbstractEvaluationRequest
    {
        public Pbreak(bool useModel = false, string model = null) : base("pbreak", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}