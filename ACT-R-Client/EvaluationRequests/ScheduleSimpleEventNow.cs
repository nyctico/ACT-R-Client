using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleEventNow : AbstractEvaluationRequest
    {
        public ScheduleSimpleEventNow(string action, object[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null) : base("schedule-simple-event-now", model)
        {
            Action = action;
            Parameters = parameters;
            Module = module;
            Priority = priority;
            Maintenance = maintenance;
        }

        public string Action { set; get; }
        public object[] Parameters { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }
        public bool Maintenance { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Action);
            parameterList.Add(Parameters);
            parameterList.Add(Module);
            parameterList.Add(Priority);
            parameterList.Add(Maintenance);
        }
    }
}