using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Buffers : AbstractEvaluationRequest
    {
        public Buffers(bool useModel = false, string model = null) : base("buffers", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}