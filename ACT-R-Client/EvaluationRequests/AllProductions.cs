﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AllProductions : AbstractEvaluationRequest
    {
        public AllProductions(string model = null) : base("all-productions", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}