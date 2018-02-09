﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetParameterValue: AbstractEvaluationRequest
    {
        public SetParameterValue(string parameter,string value, bool useModel = false, string model = null) : base("set-parameter-value", useModel,
            model)
        {
            Parameter = parameter;
            Value = value;
        }

        public string Parameter { get; set; }
        public string Value { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameter);
            list.Add(Value);

            return list;
        }
    }
}