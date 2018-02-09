using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ReleaseChunkSpecId: AbstractEvaluationRequest
    {
        public ReleaseChunkSpecId(string specId, bool useModel = false, string model = null) : base("release-chunk-spec-id", useModel,
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