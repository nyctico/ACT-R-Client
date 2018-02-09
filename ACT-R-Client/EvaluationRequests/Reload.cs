using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Reload : AbstractEvaluationRequest
    {
        public Reload(bool compile = false, string model = null) : base("reload",
            model)
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