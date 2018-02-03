using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherCommands
{
    public class PermuteList : AbstractEvalCommand
    {
        public List<int> Indexes { set; get; }
        
        public PermuteList(bool useModel = false, string model = null) : base("permute-list", useModel, model)
        {
            
        }
        
        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();

            list.Add(Indexes);

            return list;
        }
    }
}