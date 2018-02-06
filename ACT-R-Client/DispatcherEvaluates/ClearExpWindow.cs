using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class ClearExpWindow: AbstractDispatcherEvaluate
    {
        public Device Window { set; get; }

        public ClearExpWindow(Device window=null, bool useModel = false, string model = null) : base("clear-exp-window", useModel, model)
        {
            Window = window;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window);

            return list;
        }
    }
}