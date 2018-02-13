using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintDmFinsts : AbstractEvaluationRequest
    {
        public PrintDmFinsts(string model = null) : base("print-dm-finsts", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}