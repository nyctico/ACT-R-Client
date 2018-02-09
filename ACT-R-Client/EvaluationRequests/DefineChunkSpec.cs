using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunkSpec : AbstractEvaluationRequest
    {
        public DefineChunkSpec(List<dynamic> spec, string model = null) : base(
            "define-chunk-spec",
            model)
        {
            Spec = spec;
        }

        public List<dynamic> Spec { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Spec);

            return list;
        }
    }
}