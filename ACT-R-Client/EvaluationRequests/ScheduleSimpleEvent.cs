namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleSimpleEvent : AbstractEvaluationRequest
    {
        public ScheduleSimpleEvent(long time, string action, object[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false,
            string model = null) : base("schedule-simple-event", model)
        {
            Time = time;
            Action = action;
            Parameters = parameters;
            Module = module;
            Priority = priority;
            Maintenance = maintenance;
        }

        public long Time { set; get; }
        public string Action { set; get; }
        public object[] Parameters { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }
        public bool Maintenance { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(Action);
            list.Add(Parameters);
            list.Add(Module);
            list.Add(Priority);
            list.Add(Maintenance);

            return list.ToArray();
        }
    }
}