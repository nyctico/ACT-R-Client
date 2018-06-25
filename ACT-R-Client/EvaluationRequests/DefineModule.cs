using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineModule : AbstractEvaluationRequest
    {
        public DefineModule(string name, object[] buffers, object[] parameters, string version, string doc,
            string inter, string model = null) : base("define-module",
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
        public object[] Buffers { get; set; }
        public object[] Parameters { get; set; }
        public string Version { get; set; }
        public string Doc { get; set; }
        public string Inter { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Name);
            parameterList.Add(Buffers);
            parameterList.Add(Parameters);
            parameterList.Add(Version);
            parameterList.Add(Doc);
            parameterList.Add(Inter);
        }
    }
}