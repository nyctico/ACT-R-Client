using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintWarning : AbstractEvaluationRequest
    {
        public PrintWarning(string warning = null, string model = null) : base("print-warning",
            model)
        {
            Warning = warning;
        }

        public string Warning { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Warning);
        }
    }
}