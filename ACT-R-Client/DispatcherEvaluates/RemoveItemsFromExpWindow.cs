using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class RemoveItemsFromExpWindow : AbstractDispatcherEvaluate
    {
        public Device Window { set; get; }
        public List<dynamic> Items { get; set; }

        public RemoveItemsFromExpWindow(Device window, List<dynamic> items, bool useModel = false, string model = null) : base("remove-items-from-exp-window", useModel, model)
        {
            Window = window;
            Items = items;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window);
            list.Add(Items);

            return list;
        }
    }
}