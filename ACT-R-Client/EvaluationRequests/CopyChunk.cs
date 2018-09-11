using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CopyChunk : AbstractEvaluationRequest
    {
        public CopyChunk(string chunkName, string model = null) : base("copy-chunk",
            model)
        {
            ChunkName = chunkName;
        }

        public string ChunkName { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ChunkName);
        }
    }
}