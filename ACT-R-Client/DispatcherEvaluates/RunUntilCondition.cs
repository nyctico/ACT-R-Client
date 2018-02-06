using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class RunUntilCondition: AbstractDispatcherEvaluate
    {
        public string Condition { set; get; }
        public bool RealTime { set; get; }

        public RunUntilCondition(string condition, bool realTime=false, bool useModel = false, string model = null) : base("run-until-condition", useModel, model)
        {
            Condition = condition;
            RealTime = realTime;
        }
        
        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Condition);
            list.Add(RealTime);

            return list;
        }
    }
}