using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ActrOutput : AbstractEvaluationRequest
    {
        public string Output { get; set; }

        public ActrOutput(string output=null, bool useModel = false, string model = null) : base("act-r-output", useModel, model)
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