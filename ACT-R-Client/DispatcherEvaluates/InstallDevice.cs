using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class InstallDevice : AbstractDispatcherEvaluate
    {
        public InstallDevice(Device window, bool useModel = false, string model = null) : base("install-device",
            useModel, model)
        {
            Window = window;
        }

        public Device Window { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());

            return list;
        }
    }
}