using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunkSpec : AbstractEvaluationRequest
    {
        public DefineChunkSpec(dynamic[] spec, string model = null) : base(
            "define-chunk-spec",
            model)
        {
            Spec = spec;
        }

        public dynamic[] Spec { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Spec);
        }
    }
}