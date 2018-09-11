using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ScheduleEventAfterModule : AbstractEvaluationRequest
    {
        public ScheduleEventAfterModule(string afterModule, string action, dynamic[] parameters = null,
            string module = "NONE", bool maintenance = false, string destination = null, string details = null,
            bool output = true,
            bool timeInMs = false, string precondition = null, bool dynamic = false, bool delay = true,
            bool includeMaintenance = false, string model = null) : base("schedule-event-after-module", model)
        {
            AfterModule = afterModule;
            Action = action;
            Parameters = parameters;
            Module = module;
            Maintenance = maintenance;
            Destination = destination;
            Details = details;
            Output = output;
            TimeInMs = timeInMs;
            Precondition = precondition;
            Dynamic = dynamic;
            Delay = delay;
            IncludeMaintenance = includeMaintenance;
        }

        public string AfterModule { set; get; }
        public string Action { set; get; }
        public dynamic[] Parameters { set; get; }
        public string Module { set; get; }
        public bool Maintenance { set; get; }
        public string Destination { set; get; }
        public string Details { set; get; }
        public bool Output { set; get; }
        public bool TimeInMs { set; get; }
        public string Precondition { set; get; }
        public bool Dynamic { set; get; }
        public bool Delay { set; get; }
        public bool IncludeMaintenance { set; get; }

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

                var dynamicList = new List<dynamic>();
                dynamicList.Add("dynamic");
                dynamicList.Add(Dynamic);
                subParameterList.Add(dynamicList);

                var delayList = new List<dynamic>();
                delayList.Add("delay");
                delayList.Add(Delay);
                subParameterList.Add(delayList);

                var includeMaintenanceList = new List<dynamic>();
                includeMaintenanceList.Add("include-maintenance");
                includeMaintenanceList.Add(IncludeMaintenance);
                subParameterList.Add(includeMaintenanceList);

                return subParameterList;
            }
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(AfterModule);
            parameterList.Add(Action);
            parameterList.Add(SubParameterList);
        }
    }
}