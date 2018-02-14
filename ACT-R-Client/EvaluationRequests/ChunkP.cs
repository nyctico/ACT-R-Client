namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkP : AbstractEvaluationRequest
    {
        public ChunkP(string chunkName, string model = null) : base("chunk-p", model)
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