using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherCommands
{
    public class InstallDevice : AbstractEvalCommand
    {
        public List<dynamic> Window { set; get; }

        public InstallDevice(List<dynamic> window, bool useModel = false, string model = null) : base("install-device", useModel, model)
        {
            Window = window;
        }

        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Window);

            return list;
        }
    }
}