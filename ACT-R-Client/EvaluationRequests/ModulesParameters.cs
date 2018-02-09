using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesParameters : AbstractEvaluationRequest
    {
        public ModulesParameters(string module, string model = null) : base("modules-parameters",
            model)
        {
            Module = module;
        }

        public string Module { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Module);

            return list;
        }
    }
}