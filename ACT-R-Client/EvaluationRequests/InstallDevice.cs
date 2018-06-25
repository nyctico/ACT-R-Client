using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class InstallDevice : AbstractEvaluationRequest
    {
        public InstallDevice(IDevice window, string model = null) : base("install-device",
            model)
        {
            Window = window;
        }

        public IDevice Window { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Window.ToJsonList());
        }
    }
}