using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintDmFinsts: AbstractEvaluationRequest
    {
        public PrintDmFinsts(bool useModel = false, string model = null) : base("print-dm-finsts", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}