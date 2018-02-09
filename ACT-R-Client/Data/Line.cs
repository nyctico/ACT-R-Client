using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Line : IItem
    {
        public Line(string windowTitle, string id)
        {
            WindowTitle = windowTitle;
            ID = id;
        }

        public string WindowTitle { set; get; }
        public string ID { set; get; }

        public List<dynamic> ToJsonList()
        {
            var list = new List<dynamic>();

            list.Add(WindowTitle);
            list.Add(ID);

            return list;
        }
    }
}