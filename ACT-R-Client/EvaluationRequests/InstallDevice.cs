using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class InstallDevice : AbstractEvaluationRequest
    {
        public InstallDevice(Window window, bool useModel = false, string model = null) : base("install-device",
            useModel, model)
        {
            Window = window;
        }

        public Window Window { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());

            return list;
        }
    }
}