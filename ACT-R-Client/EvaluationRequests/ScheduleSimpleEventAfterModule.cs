using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleEventAfterModule : AbstractEvaluationRequest
    {
        public ScheduleSimpleEventAfterModule(string afterModule, string action, object[] parameters = null,
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
        public object[] Parameters { set; get; }
        public string Module { set; get; }
        public bool Maintenance { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(AfterModule);
            parameterList.Add(Action);
            parameterList.Add(Parameters);
            parameterList.Add(Module);
            parameterList.Add(Maintenance);
        }
    }
}