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

        public object[] ToJsonList()
        {
            var list = new List<object>();

            list.Add(WindowTitle);
            list.Add(Id);

            return list.ToArray();
        }
    }
}