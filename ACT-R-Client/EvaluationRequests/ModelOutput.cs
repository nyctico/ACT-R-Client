using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModelOutput: AbstractEvaluationRequest
    {
        public string Output { get; set; }

        public ModelOutput(string output, bool useModel = false, string model = null) : base("model-output", useModel, model)
        {
            Output = output;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Output);

            return list;
        }
    }
}