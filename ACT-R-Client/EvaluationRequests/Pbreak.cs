﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Pbreak : AbstractEvaluationRequest
    {
        public Pbreak(List<string> productionNames, string model = null) : base("pbreak", model)
        {
            ProductionNames = productionNames;
        }

        public List<string> ProductionNames { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(ProductionNames);

            return list.ToArray();
        }
    }
}