﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModulesWithParameters : AbstractEvaluationRequest
    {
        public ModulesWithParameters(string model = null) : base("modules-with-parameters",
            model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}