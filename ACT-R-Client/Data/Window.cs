using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Window : IDevice
    {
        public Window(string modul, string method, string title)
        {
            Modul = modul;
            Method = method;
            Title = title;
        }

        public string Modul { set; get; }
        public string Method { set; get; }
        public string Title { set; get; }

        public List<dynamic> ToJsonList()
        {
            var list = new List<dynamic>();

            list.Add(Modul);
            list.Add(Method);
            list.Add(Title);

            return list;
        }
    }
}