using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherMonitors
{
    public class DispatcherMonitor
    {
        public DispatcherMonitor(string commandToMonitor, string commandToCall, string monitorStyle = null)
        {
            CommandToMonitor = commandToMonitor;
            CommandToCall = commandToCall;
            MonitorStyle = monitorStyle;
        }

        public string CommandToMonitor { get; }
        public string CommandToCall { get; }
        public string MonitorStyle { set; get; }

        public List<dynamic> ToParameterList()
        {
            var list = new List<dynamic> {CommandToMonitor, CommandToCall};

            if (MonitorStyle != null) list.Add(MonitorStyle);

            return list;
        }
    }
}