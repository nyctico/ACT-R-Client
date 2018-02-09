using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedAudicon : AbstractEvaluationRequest
    {
        public PrintedAudicon(bool useModel = false, string model = null) : base("printed-audicon", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}