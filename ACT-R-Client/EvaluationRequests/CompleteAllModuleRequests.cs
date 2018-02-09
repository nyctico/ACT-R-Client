using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteAllModuleRequests : AbstractEvaluationRequest
    {
        public CompleteAllModuleRequests(string moduleName, bool useModel = false,
            string model = null) : base("complete-all-module-requests", useModel, model)
        {
            ModuleName = moduleName;
        }

        public string ModuleName { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ModuleName);

            return list;
        }
    }
}