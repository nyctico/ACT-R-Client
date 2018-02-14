using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddTextToWindow : AbstractEvaluationRequest
    {
        public AddTextToWindow(Window window, string text, int x, int y, string color = "black", int height = 50,
            int width = 75,
            int fontSize = 12, string model = null) : base("add-text-to-exp-window",
            model)
        {
            Window = window;
            Text = text;
            X = x;
            Y = y;
            Color = color;
            Height = height;
            Width = width;
            FontSize = fontSize;
        }

        public Window Window { set; get; }
        public string Text { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public string Color { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public int FontSize { set; get; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());
            list.Add(Text);
            list.Add(X);
            list.Add(Y);
            list.Add(Color);
            list.Add(Height);
            list.Add(Width);
            list.Add(FontSize);

            return list.ToArray();
        }
    }
}