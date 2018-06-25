using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Pp : AbstractEvaluationRequest
    {
        public Pp(List<string> parameters, string model = null) : base("pp",
            model)
        {
            Parameters = parameters;
        }

        public List<string> Parameters { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Parameters);
        }
    }
}