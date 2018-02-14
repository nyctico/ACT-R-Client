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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkSpecId);

            return list.ToArray();
        }
    }
}