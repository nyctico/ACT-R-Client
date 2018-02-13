using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesParameters : AbstractEvaluationRequest
    {
        public ModulesParameters(string moduleName, string model = null) : base("modules-parameters",
            model)
        {
            ModuleName = moduleName;
        }

        public string ModuleName { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ModuleName);

            return list;
        }
    }
}