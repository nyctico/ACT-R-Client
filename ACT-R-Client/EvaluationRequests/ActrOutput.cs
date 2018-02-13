using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ActrOutput : AbstractEvaluationRequest
    {
        public ActrOutput(string output, string model = null) : base("act-r-output",
            model)
        {
            Output = output;
        }

        public string Output { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Output);

            return list;
        }
    }
}