using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunks : AbstractEvaluationRequest
    {
        public DefineChunks(dynamic[] chunks, string model = null) : base("define-chunks",
            model)
        {
            Chunks = chunks;
        }

        public dynamic[] Chunks { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Chunks);
        }
    }
}