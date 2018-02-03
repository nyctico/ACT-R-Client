﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Device
    {
        public Device(List<dynamic> infomation)
        {
            Infomation = infomation;
        }

        public List<dynamic> Infomation { get; }

        public List<dynamic> ToJsonList()
        {
            return Infomation;
        }
    }
}