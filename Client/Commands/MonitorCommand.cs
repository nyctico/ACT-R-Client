using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class MonitorCommand
    {
        public string CommandToMonitor { get; }
        public string CommandToCall { get; }
        public string MonitorStyle { set; get; }

        public MonitorCommand(string commandToMonitor, string commandToCall)
        {
            CommandToMonitor = commandToMonitor;
            CommandToCall = commandToCall;
        }
        
        public List<dynamic> ToParameterList()
        {
            List<dynamic> list = new List<dynamic>();
            
            list.Add(CommandToMonitor);
            list.Add(CommandToCall);
            if (MonitorStyle != null) list.Add(MonitorStyle);
            
            return list;
        }
    }
}