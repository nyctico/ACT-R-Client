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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());

            return list.ToArray();
        }
    }
}