using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Text : IItem
    {
        public Text(string windowTitle, string id)
        {
            WindowTitle = windowTitle;
            Id = id;
        }

        public string WindowTitle { set; get; }
        public string Id { set; get; }

        public dynamic[] ToJsonList()
        {
            var list = new List<dynamic>();

            list.Add(WindowTitle);
            list.Add(Id);

            return list.ToArray();
        }
    }
}