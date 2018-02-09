using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PurgeChunk : AbstractEvaluationRequest
    {
        public PurgeChunk(string name, bool useModel = false, string model = null) : base("purge-chunk", useModel,
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