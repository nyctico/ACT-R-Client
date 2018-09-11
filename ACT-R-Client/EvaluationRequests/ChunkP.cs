using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkP : AbstractEvaluationRequest
    {
        public ChunkP(string chunkName, string model = null) : base("chunk-p", model)
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