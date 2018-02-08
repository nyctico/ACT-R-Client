using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public interface IItem
    {
        List<dynamic> ToJsonList();
    }
}