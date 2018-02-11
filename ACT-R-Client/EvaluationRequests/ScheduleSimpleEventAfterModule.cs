﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleEventAfterModule : AbstractEvaluationRequest
    {
        public ScheduleSimpleEventAfterModule(string afterModule, string action, List<dynamic> parameters = null,
            string module = "NONE", bool maintenance = false,
            string model = null) : base("schedule-simple-event-after-module", model)
        {
            AfterModule = afterModule;
            Action = action;
            Parameters = parameters;
            Module = module;
            Maintenance = maintenance;
        }

        public string AfterModule { set; get; }
        public string Action { set; get; }
        public List<dynamic> Parameters { set; get; }
        public string Module { set; get; }
        public bool Maintenance { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(AfterModule);
            list.Add(Action);
            list.Add(Parameters);
            list.Add(Module);
            list.Add(Maintenance);

            return list;
        }
    }
}