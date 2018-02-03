using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class Run : AbstractEvalCommand
    {
        public int Time { set; get; }
        public bool RealTime { set; get; }

        public Run(int time, bool realTime, bool useModel = false, string model = null) : base("run", useModel, model)
        {
            Time = time;
            RealTime = realTime;
        }

        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Time);
            list.Add(RealTime);

            return list;
        }
    }
}