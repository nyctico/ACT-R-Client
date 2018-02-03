using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherCommands
{
    public class OpenExpWindow : AbstractEvalCommand
    {
        public string Title { set; get; }
        public bool Visible { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int X { set; get; }
        public int Y { set; get; }

        public OpenExpWindow(string title, bool visible, int width, int height, int x, int y, bool useModel = false, string model = null) : base("open-exp-window", useModel, model)
        {
            Title = title;
            Visible = visible;
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Title);
            list.Add(Visible);
            list.Add(Width);
            list.Add(Height);
            list.Add(X);
            list.Add(Y);

            return list;
        }
    }
}