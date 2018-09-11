using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleEventRelative : AbstractEvaluationRequest
    {
        public ScheduleEventRelative(long timeDelay, string action, dynamic[] parameters = null,
            string module = "NONE", int priority = 0, bool maintenance = false, string destination = null,
            string details = null, bool output = true,
            bool timeInMs = false, string precondition = null, string model = null) : base("schedule-event-relative",
            model)
        {
            TimeDelay = timeDelay;
            Action = action;
            Parameters = parameters;
            Module = module;
            Priority = priority;
            Maintenance = maintenance;
            Destination = destination;
            Details = details;
            Output = output;
            TimeInMs = timeInMs;
            Precondition = precondition;
        }

        public long TimeDelay { set; get; }
        public string Action { set; get; }
        public dynamic[] Parameters { set; get; }
        public string Module { set; get; }
        public int Priority { set; get; }
        public bool Maintenance { set; get; }
        public string Destination { set; get; }
        public string Details { set; get; }
        public bool Output { set; get; }
        public bool TimeInMs { set; get; }
        public string Precondition { set; get; }

        private List<dynamic> SubParameterList
        {
            get
            {
                var subParameterList = new List<dynamic>();

                var paramsList = new List<dynamic>();
                paramsList.Add("params");
                paramsList.Add(Parameters);
                subParameterList.Add(paramsList);

                var moduleList = new List<dynamic>();
                moduleList.Add("module");
                moduleList.Add(Module);
                subParameterList.Add(moduleList);

                var priorityList = new List<dynamic>();
                priorityList.Add("priority");
                priorityList.Add(Priority);
                subParameterList.Add(priorityList);

                var maintenanceList = new List<dynamic>();
                maintenanceList.Add("maintenance");
                maintenanceList.Add(Maintenance);
                subParameterList.Add(maintenanceList);

                var destinationList = new List<dynamic>();
                destinationList.Add("destination");
                destinationList.Add(Destination);
                subParameterList.Add(destinationList);

                var detailsList = new List<dynamic>();
                detailsList.Add("details");
                detailsList.Add(Details);
                subParameterList.Add(detailsList);

                var outputList = new List<dynamic>();
                outputList.Add("output");
                outputList.Add(Output);
                subParameterList.Add(outputList);

                var timeInMsList = new List<dynamic>();
                timeInMsList.Add("time-in-ms");
                timeInMsList.Add(TimeInMs);
                subParameterList.Add(timeInMsList);

                var preconditionList = new List<dynamic>();
                preconditionList.Add("precondition");
                preconditionList.Add(Precondition);
                subParameterList.Add(preconditionList);

                return subParameterList;
            }
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(TimeDelay);
            parameterList.Add(Action);
            parameterList.Add(SubParameterList);
        }
    }
}