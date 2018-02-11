using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteRequest : AbstractEvaluationRequest
    {
        public CompleteRequest(string chunkSpecId, string model = null) : base("complete-request",
            model)
        {
            ChunkSpecId = chunkSpecId;
        }

        public string ChunkSpecId { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkSpecId);

            return list;
        }
    }
}