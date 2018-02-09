using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkSpecToChunkDef: AbstractEvaluationRequest
    {
        public ChunkSpecToChunkDef(string specId, bool useModel = false, string model = null) : base("chunk-spec-to-chunk-def", useModel,
            model)
        {
            SpecId = specId;
        }

        public string SpecId { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(SpecId);

            return list;
        }
    }
}