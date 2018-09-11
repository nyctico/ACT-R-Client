using System.Collections.Generic;

namespace Nyctico.Actr.Client.AddCommandRequests
{
    public abstract class AbstractAddCommandRequest
    {
        protected AbstractAddCommandRequest(string publishedName, string privateName,
            string documentation = "No documentation provided.",
            string multipleInstanceErrorMessage = null, string lispCmd = null)
        {
            PublishedName = publishedName;
            PrivateName = privateName;
            Documentation = documentation;
            MultipleInstanceErrorMessage = multipleInstanceErrorMessage;
            LispCmd = lispCmd;
        }

        public string PublishedName { get; set; }

        public dynamic[] PublishedNameAsList => new dynamic[] {PublishedName};

        public string PrivateName { set; get; }
        public string Documentation { set; get; }
        public string MultipleInstanceErrorMessage { set; get; }
        public string LispCmd { set; get; }

        public dynamic[] ToParameterList()
        {
            var list = new List<dynamic>();

            list.Add(PublishedName);
            list.Add(PrivateName);
            list.Add(Documentation);
            if (MultipleInstanceErrorMessage == null) list.Add(true);
            else list.Add(MultipleInstanceErrorMessage);
            if (LispCmd != null) list.Add(LispCmd);

            return list.ToArray();
        }

        public abstract void Execute(dynamic[] parameters);
    }
}