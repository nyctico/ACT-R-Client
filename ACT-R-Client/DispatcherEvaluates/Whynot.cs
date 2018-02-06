using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class Whynot : AbstractDispatcherEvaluate
    {
        public List<dynamic> Parameters { get; set; }

        public Whynot(List<dynamic> parameters, bool useModel = false, string model = null) : base("whynot", useModel, model)
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