using System.Collections.Generic;
using System.Net.Mime;

namespace Nyctico.Actr.Client.Data
{
    public class Run : AbstractEvalCommand
    {
        public int Time { set; get; }
        public bool RealTime { set; get; }
        
        public Run(bool useModel = false, string model = null) : base("run", useModel, model)
        {
            
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