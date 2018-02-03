using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherCommands
{
    public class InstallDevice : AbstractEvalCommand
    {
        public Device Window { set; get; }

        public InstallDevice(Device window, bool useModel = false, string model = null) : base("install-device", useModel, model)
        {
            Window = window;
        }

        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Window.ToJsonList());

            return list;
        }
    }
}