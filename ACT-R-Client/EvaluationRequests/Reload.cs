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

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Compile);

            return list;
        }
    }
}