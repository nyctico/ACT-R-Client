using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PurgeChunk : AbstractEvaluationRequest
    {
        public PurgeChunk(string chunkName, string model = null) : base("purge-chunk",
            model)
        {
            ChunkName = chunkName;
        }

        public string ChunkName { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list;
        }
    }
}