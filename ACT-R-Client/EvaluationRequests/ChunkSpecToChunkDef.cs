using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkSpecToChunkDef : AbstractEvaluationRequest
    {
        public ChunkSpecToChunkDef(string chunkSpecId, string model = null) : base(
            "chunk-spec-to-chunk-def",
            model)
        {
            ChunkSpecId = chunkSpecId;
        }

        public string ChunkSpecId { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ChunkSpecId);
        }
    }
}