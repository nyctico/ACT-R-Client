﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CommandOutput: AbstractEvaluationRequest
    {
        public CommandOutput(string command = null, bool useModel = false, string model = null) : base("command-output",
            useModel, model)
        {
            command = command;
        }

        public string command { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(command);

            return list;
        }
    }
}