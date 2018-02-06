using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class Penable: AbstractDispatcherEvaluate
    {
        public List<dynamic> Parameters { get; set; }

        public Penable(List<dynamic> parameters, bool useModel = false, string model = null) : base("penable", useModel, model)
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