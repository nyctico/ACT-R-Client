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

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Name);
            list.Add(Buffers);
            list.Add(Parameters);
            list.Add(Version);
            list.Add(Doc);
            list.Add(Inter);

            return list.ToArray();
        }
    }
}