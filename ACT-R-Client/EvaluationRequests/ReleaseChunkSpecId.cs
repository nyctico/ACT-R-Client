using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ReleaseChunkSpecId : AbstractEvaluationRequest
    {
        public ReleaseChunkSpecId(string chunkSpecId, string model = null) : base(
            "release-chunk-spec-id",
            model)
        {
            ChunkSpecId = chunkSpecId;
        }

        public string ChunkSpecId { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkSpecId);

            return list;
        }
    }
}