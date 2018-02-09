using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesWithParameters : AbstractEvaluationRequest
    {
        public ModulesWithParameters(bool useModel = false, string model = null) : base("modules-with-parameters",
            useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}