using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleEventNow : AbstractEvaluationRequest
    {
        public ScheduleSimpleEventNow(string action, List<dynamic> parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, bool useModel = false,
            string model = null) : base("schedule-simple-event-now", useModel, model)
        {
            Action = action;
            Parameters = parameters;
            Module = module;
            Priority = priority;
            Maintenance = maintenance;
        }

        public string Action { set; get; }
        public List<dynamic> Parameters { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }
        public bool Maintenance { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Action);
            list.Add(Parameters);
            list.Add(Module);
            list.Add(Priority);
            list.Add(Maintenance);

            return list;
        }
    }
}