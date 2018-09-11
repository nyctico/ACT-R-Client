using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddLineToExpWindow : AbstractEvaluationRequest
    {
        public AddLineToExpWindow(Window window, int[] start, int[] end, string color = null,
            string model = null) : base("add-line-to-exp-window",
            model)
        {
            Window = window;
            Start = start;
            End = end;
            Color = color;
        }

        public Window Window { set; get; }
        public int[] Start { set; get; }
        public int[] End { set; get; }
        public string Color { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Window.ToJsonList());
            parameterList.Add(Start);
            parameterList.Add(End);
            if (Color != null) parameterList.Add(Color);
        }
    }
}