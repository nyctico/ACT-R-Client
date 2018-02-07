using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkP: AbstractEvaluationRequest
    {
        public string ChunkName { get; set; }

        public ChunkP(string chunkName, bool useModel = false, string model = null) : base("chunk-p", useModel, model)
        {
            ChunkName = chunkName;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list;
        }
    }
}