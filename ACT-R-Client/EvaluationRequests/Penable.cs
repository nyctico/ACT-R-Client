﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Penable : AbstractEvaluationRequest
    {
        public Penable(List<string> productionNames, string model = null) : base("penable",
            model)
        {
            ProductionNames = productionNames;
        }

        public List<string> ProductionNames { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ProductionNames);
        }
    }
}