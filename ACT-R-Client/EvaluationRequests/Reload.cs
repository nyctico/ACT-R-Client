using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Reload : AbstractEvaluationRequest
    {
        public bool Compile { get; set; }

        public Reload(bool compile=false, bool useModel = false, string model = null) : base("reload", useModel, model)
        {
            Compile = compile;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Compile);

            return list;
        }
    }
}