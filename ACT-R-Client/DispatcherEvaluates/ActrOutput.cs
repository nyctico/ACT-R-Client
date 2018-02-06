using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class ActrOutput : AbstractDispatcherEvaluate
    {
        public string Output { get; set; }

        public ActrOutput(string output=null, bool useModel = false, string model = null) : base("act-r-output", useModel, model)
        {
            Output = output;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Output);

            return list;
        }
    }
}