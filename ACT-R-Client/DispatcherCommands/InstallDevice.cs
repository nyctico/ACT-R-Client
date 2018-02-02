using System.Collections.Generic;

namespace Nyctico.Actr.Client.Data
{
    public class InstallDevice : AbstractEvalCommand
    {
        public List<dynamic> Window { set; get; }
        
        public InstallDevice(bool useModel = false, string model = null) : base("install-device", useModel, model)
        {
            
        }
        
        public override List<dynamic> ToParameterList()
        {
            List<dynamic> list = BaseParameterList();
            
            list.Add(Window);

            return list;
        }
    }
}