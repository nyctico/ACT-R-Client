using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class UsedProductionBuffers : AbstractEvaluationRequest
    {
        public UsedProductionBuffers(bool useModel = false, string model = null) : base("used-production-buffers",
            useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}