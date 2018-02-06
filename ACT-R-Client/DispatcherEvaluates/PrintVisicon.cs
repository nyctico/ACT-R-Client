using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class PrintVisicon: AbstractDispatcherEvaluate
    {

        public PrintVisicon(bool useModel = false, string model = null) : base("print-visicon", useModel, model)
        {
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}