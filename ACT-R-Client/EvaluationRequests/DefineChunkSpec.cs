using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunkSpec : AbstractEvaluationRequest
    {
        public DefineChunkSpec(List<object> spec, string model = null) : base(
            "define-chunk-spec",
            model)
        {
            Spec = spec;
        }

        public List<object> Spec { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Spec);

            return list;
        }
    }
}