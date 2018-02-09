using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PurgeChunk : AbstractEvaluationRequest
    {
        public PurgeChunk(string name, string model = null) : base("purge-chunk",
            model)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Name);

            return list;
        }
    }
}