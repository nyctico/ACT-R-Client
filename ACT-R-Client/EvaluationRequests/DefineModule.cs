using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineModule : AbstractEvaluationRequest
    {
        public DefineModule(string name, List<dynamic> buffers, List<dynamic> parameters, string version, string doc,
            string inter, bool useModel = false, string model = null) : base("define-module", useModel,
            model)
        {
            Name = name;
            Buffers = buffers;
            Parameters = parameters;
            Version = version;
            Doc = doc;
            Inter = inter;
        }

        public string Name { get; set; }
        public List<dynamic> Buffers { get; set; }
        public List<dynamic> Parameters { get; set; }
        public string Version { get; set; }
        public string Doc { get; set; }
        public string Inter { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Name);
            list.Add(Buffers);
            list.Add(Parameters);
            list.Add(Version);
            list.Add(Doc);
            list.Add(Inter);

            return list;
        }
    }
}