using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class Pdisable: AbstractDispatcherEvaluate
    {
        public List<dynamic> Parameters { get; set; }

        public Pdisable(List<dynamic> parameters, bool useModel = false, string model = null) : base("pdisable", useModel, model)
        {
            Parameters = parameters;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}