using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ReleaseChunkSpecId : AbstractEvaluationRequest
    {
        public ReleaseChunkSpecId(string specId, string model = null) : base(
            "release-chunk-spec-id",
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