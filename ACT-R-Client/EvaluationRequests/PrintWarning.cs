﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintWarning : AbstractEvaluationRequest
    {
        public PrintWarning(string warning = null, string model = null) : base("print-warning",
            model)
        {
            Warning = warning;
        }

        public string Warning { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Warning);

            return list;
        }
    }
}