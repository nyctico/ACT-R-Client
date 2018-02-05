using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class InstallDevice : AbstractDispatcherEvaluate
    {
        public InstallDevice(Device device, bool useModel = false, string model = null) : base("install-device",
            useModel, model)
        {
            Device = device;
        }

        public Device Device { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Device.ToJsonList());

            return list;
        }
    }
}