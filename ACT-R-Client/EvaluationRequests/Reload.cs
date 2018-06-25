using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Reload : AbstractEvaluationRequest
    {
        public Reload(bool compile = false) : base("reload")
        {
            Compile = compile;
        }

        public bool Compile { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Compile);
        }
    }
}