using System.Collections.Generic;

namespace Nyctico.Actr.Client.MonitorRequests
{
    public class MonitorRequest
    {
        public MonitorRequest(string commandToMonitor, string commandToCall, string monitorStyle = null)
        {
            CommandToMonitor = commandToMonitor;
            CommandToCall = commandToCall;
            MonitorStyle = monitorStyle;
        }

        public string CommandToMonitor { get; set; }
        public string CommandToCall { get; set; }
        public string MonitorStyle { set; get; }

        public object[] ToParameterList()
        {
            var list = new List<object> {CommandToMonitor, CommandToCall};

            if (MonitorStyle != null) list.Add(MonitorStyle);

            return list.ToArray();
        }
    }
}