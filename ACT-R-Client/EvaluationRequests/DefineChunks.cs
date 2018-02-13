using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunks : AbstractEvaluationRequest
    {
        public DefineChunks(List<object> chunks, string model = null) : base("define-chunks",
            model)
        {
            Chunks = chunks;
        }

        public List<object> Chunks { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list;
        }
    }
}