using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class AddTextToWindow : AbstractEvalCommand
    {
        public AddTextToWindow(Device window, string text, int x, int y, string color, int height, int width,
            int fontSize, bool useModel = false, string model = null) : base("add-text-to-exp-window", useModel, model)
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

        public Device Window { set; get; }
        public string Text { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public string Color { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public int FontSize { set; get; }

        public override List<dynamic> ToParameterList()
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

            return list;
        }
    }
}