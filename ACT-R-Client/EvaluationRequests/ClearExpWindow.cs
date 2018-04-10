using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ClearExpWindow : AbstractEvaluationRequest
    {
        public ClearExpWindow(string windowTitle = null, string model = null) : base(
            "clear-exp-window", model)
        {
            windowTitle = windowTitle;
        }

        public string WindowTitle { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(WindowTitle);

            return list.ToArray();
        }
    }
}