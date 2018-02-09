using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesParameters : AbstractEvaluationRequest
    {
        public ModulesParameters(string module, bool useModel = false, string model = null) : base("modules-parameters",
            useModel,
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