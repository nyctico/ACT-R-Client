using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Line : IItem
    {
        public Line(string windowTitle, string id)
        {
            WindowTitle = windowTitle;
            Id = id;
        }

        public string WindowTitle { set; get; }
        public string Id { set; get; }

        public List<dynamic> ToJsonList()
        {
            var list = new List<dynamic>();

            list.Add(WindowTitle);
            list.Add(Id);

            return list;
        }
    }
}