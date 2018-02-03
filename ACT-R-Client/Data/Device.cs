using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class Device
    {
        public List<dynamic> device { get; }

        public Device(List<dynamic> device)
        {
            this.device = device;
        }
        
        public List<dynamic> ToJsonList()
        {
            return device;
        }
    }
}