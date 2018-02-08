﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Device
    {
        public Device(string modul, string method, string title)
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
            List<dynamic> list = new List<dynamic>();

            list.Add(Modul);
            list.Add(Method);
            list.Add(Title);

            return list;
        }
    }
}