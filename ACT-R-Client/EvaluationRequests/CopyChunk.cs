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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list.ToArray();
        }
    }
}