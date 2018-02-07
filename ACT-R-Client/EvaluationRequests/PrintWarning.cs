using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintWarning: AbstractEvaluationRequest
    {
        public string Warning { get; set; }

        public PrintWarning(string warning=null, bool useModel = false, string model = null) : base("print-warning", useModel, model)
        {
            Warning = warning;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Warning);

            return list;
        }
    }
}