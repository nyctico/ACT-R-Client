using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RemoveItemFromExpWindow : AbstractEvaluationRequest
    {
        public RemoveItemFromExpWindow(Window window, IItem item, string model = null)
            : base("remove-items-from-exp-window", model)
        {
            Window = window;
            Item = item;
        }

        public Window Window { set; get; }
        public IItem Item { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());
            list.Add(Item.ToJsonList());

            return list;
        }
    }
}