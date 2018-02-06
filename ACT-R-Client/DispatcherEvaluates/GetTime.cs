using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class GetTime: AbstractDispatcherEvaluate
    {
        public bool ModelTime { get; set; }

        public GetTime(bool modelTime=true, bool useModel = false, string model = null) : base("get-time", useModel, model)
        {
            ModelTime = modelTime;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ModelTime);

            return list;
        }
    }
}