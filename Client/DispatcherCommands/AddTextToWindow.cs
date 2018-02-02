using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class AddTextToWindow : AbstractEvalCommand
    {
        public List<dynamic> Window { set; get; }
        public string Text { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public string Color { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public int FontSize { set; get; }
        
        public AddTextToWindow(bool useModel = false, string model = null) : base("add-text-to-exp-window", useModel, model)
        {
            
        }
        
        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Window);
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