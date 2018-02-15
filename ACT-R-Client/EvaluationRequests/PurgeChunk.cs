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

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list.ToArray();
        }
    }
}