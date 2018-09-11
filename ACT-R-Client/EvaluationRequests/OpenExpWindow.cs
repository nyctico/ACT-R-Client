using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class OpenExpWindow : AbstractEvaluationRequest
    {
        public OpenExpWindow(string title, bool visible, int width = 300, int height = 300, int x = 300, int y = 300,
            string model = null) : base("open-exp-window", model)
        {
            Title = title;
            Visible = visible;
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public string Title { set; get; }
        public bool Visible { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int X { set; get; }
        public int Y { set; get; }

        private List<dynamic> SubParameterList
        {
            get
            {
                var subParameterList = new List<dynamic>();

                var visibleList = new List<dynamic>();
                visibleList.Add("visible");
                visibleList.Add(Visible);
                subParameterList.Add(visibleList);

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

                return subParameterList;
            }
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Title);
            parameterList.Add(SubParameterList);
        }
    }
}