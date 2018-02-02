﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.Commands
{
    public class MonitorCommand
    {
        public string CommandToMonitor { get; }
        public string CommandToCall { get; }
        public string MonitorStyle { set; get; }

        public MonitorCommand(string commandToMonitor, string commandToCall, string monitorStyle = null)
        {
            CommandToMonitor = commandToMonitor;
            CommandToCall = commandToCall;
            MonitorStyle = monitorStyle;
        }
        
        public List<dynamic> ToParameterList()
        {
            List<dynamic> list = new List<dynamic> {CommandToMonitor, CommandToCall};

            if (MonitorStyle != null) list.Add(MonitorStyle);
            
            return list;
        }
    }
}