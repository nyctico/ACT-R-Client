using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherHooks
{
    public abstract class AbstractDispatcherHook
    {
        private string PublishedName { set; get; }
        public List<object> PublishedNameAsList => new List<object> {PublishedName};
        public string PrivateName { set; get; }
        public string Documentation { set; get; }
        public string MultipleInstanceErrorMessage { set; get; }
        public string LispCmd { set; get; }

        protected AbstractDispatcherHook(string publishedName, string privateName, string documentation, string multipleInstanceErrorMessage=null, string lispCmd=null)
        {
            PublishedName = publishedName;
            PrivateName = privateName;
            Documentation = documentation;
            MultipleInstanceErrorMessage = multipleInstanceErrorMessage;
            LispCmd = lispCmd;
        }

        public List<dynamic> ToParameterList()
        {
            List<dynamic> list = new List<dynamic>();
            
            list.Add(PublishedName);
            list.Add(PrivateName);
            list.Add(Documentation);
            if (MultipleInstanceErrorMessage == null) list.Add(true); else list.Add(MultipleInstanceErrorMessage);
            if (LispCmd != null) list.Add(LispCmd);

            return list;
        }

        public abstract void Execute(List<dynamic> parameters);
    }
}