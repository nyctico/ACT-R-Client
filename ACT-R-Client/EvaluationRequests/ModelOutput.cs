using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModelOutput : AbstractEvaluationRequest
    {
        public ModelOutput(string output, string model = null) : base("model-output",
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