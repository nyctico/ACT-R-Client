using System.Collections.Generic;

namespace Nyctico.Actr.Client.Commands
{
    public abstract class AbstractCommand
    {
        public string PublishedName { set; get; }
        public List<object> PublishedNameAsList => new List<object> {PublishedName};
        public string PrivateName { set; get; }
        public string Documentation { set; get; }
        public string SingelInstance { set; get; }
        public string LispCmd { set; get; }
        
        public List<dynamic> ToParameterList()
        {
            List<dynamic> list = new List<dynamic>();
            
            list.Add(PublishedName);
            list.Add(PrivateName);
            list.Add(Documentation);
            if (SingelInstance != null) list.Add(SingelInstance);
            if (LispCmd != null) list.Add(LispCmd);

            return list;
        }

        public abstract void Execute(List<dynamic> parameters);
    }
}