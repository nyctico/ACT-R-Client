using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DeleteChunk : AbstractEvaluationRequest
    {
        public DeleteChunk(string chunkName, string model = null) : base("delete-chunk",
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