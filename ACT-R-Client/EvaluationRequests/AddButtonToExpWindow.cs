using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddButtonToExpWindow : AbstractEvaluationRequest
    {
        public AddButtonToExpWindow(Window window, string text, int x, int y, List<dynamic> action = null,
            int height = 50,
            int width = 75,
            string color = "gray", string model = null) : base("add-button-to-exp-window",
            model)
        {
            Window = window;
            Text = text;
            X = x;
            Y = y;
            Action = action;
            Height = height;
            Width = width;
            Color = color;
        }

        public Window Window { set; get; }
        public string Text { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public List<dynamic> Action { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public string Color { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());
            list.Add(Text);
            list.Add(X);
            list.Add(Y);
            list.Add(Action);
            list.Add(Height);
            list.Add(Width);
            list.Add(Color);

            return list;
        }
    }
}