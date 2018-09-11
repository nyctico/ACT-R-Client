using System.Collections.Generic;
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

        private List<dynamic> SubParameterList
        {
            get
            {
                var subParameterList = new List<dynamic>();

                var colorList = new List<dynamic>();
                colorList.Add("color");
                colorList.Add(Color);
                subParameterList.Add(colorList);

                var widthList = new List<dynamic>();
                widthList.Add("width");
                widthList.Add(Width);
                subParameterList.Add(widthList);

                var heightList = new List<dynamic>();
                heightList.Add("height");
                heightList.Add(Height);
                subParameterList.Add(heightList);

                var xList = new List<dynamic>();
                xList.Add("x");
                xList.Add(X);
                subParameterList.Add(xList);

                var yList = new List<dynamic>();
                yList.Add("y");
                yList.Add(Y);
                subParameterList.Add(yList);

                var fontSizeList = new List<dynamic>();
                fontSizeList.Add("font-size");
                fontSizeList.Add(FontSize);
                subParameterList.Add(fontSizeList);

                return subParameterList;
            }
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Window.ToJsonList());
            parameterList.Add(Text);
            parameterList.Add(SubParameterList);
        }
    }
}