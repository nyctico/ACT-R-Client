﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintedParameterDetails: AbstractEvaluationRequest
    {
        public PrintedParameterDetails(string parameter, bool useModel = false, string model = null) : base("printed-parameter-details", useModel,
            model)
        {
            Parameter = parameter;
        }

        public string Parameter { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameter);

            return list;
        }
    }
}