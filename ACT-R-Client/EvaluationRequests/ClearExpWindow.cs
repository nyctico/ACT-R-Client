using System.Collections.Generic;

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

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(WindowTitle);
        }
    }
}