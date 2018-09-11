using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteAllModuleRequests : AbstractEvaluationRequest
    {
        public CompleteAllModuleRequests(string moduleName,
            string model = null) : base("complete-all-module-requests", model)
        {
            ModuleName = moduleName;
        }

        public string ModuleName { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ModuleName);
        }
    }
}