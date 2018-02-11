using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CommandOutput : AbstractEvaluationRequest
    {
        public CommandOutput(string output = null, string model = null) : base("command-output",
            model)
        {
            Output = output;
        }

        public string Output { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Output);

            return list;
        }
    }
}