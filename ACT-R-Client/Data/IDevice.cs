using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public interface IDevice
    {
        List<dynamic> ToJsonList();
    }
}