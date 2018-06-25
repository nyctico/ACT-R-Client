using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunks : AbstractEvaluationRequest
    {
        public DefineChunks(object[] chunks, string model = null) : base("define-chunks",
            model)
        {
            Chunks = chunks;
        }

        public object[] Chunks { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Chunks);
        }
    }
}