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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Action);
            list.Add(Parameters);
            list.Add(Module);
            list.Add(Priority);
            list.Add(Maintenance);

            return list.ToArray();
        }
    }
}