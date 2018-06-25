using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class UndefineModule : AbstractEvaluationRequest
    {
        public UndefineModule(string moduleName, string model = null) : base("undefine-module",
            model)
        {
            ModuleName = moduleName;
        }

        public string ModuleName { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ModuleName);
        }
    }
}