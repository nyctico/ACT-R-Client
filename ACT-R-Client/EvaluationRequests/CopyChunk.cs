using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CopyChunk : AbstractEvaluationRequest
    {
        public CopyChunk(string chunkName, bool useModel = false, string model = null) : base("copy-chunk", useModel,
            model)
        {
            ChunkName = chunkName;
        }

        public string ChunkName { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list;
        }
    }
}