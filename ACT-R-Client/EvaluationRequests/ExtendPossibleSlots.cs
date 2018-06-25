using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ExtendPossibleSlots : AbstractEvaluationRequest
    {
        public ExtendPossibleSlots(string chunkName, bool warn = true, string model = null) :
            base("extend-possible-slots", model)
        {
            ChunkName = chunkName;
            Warn = warn;
        }

        public string ChunkName { get; set; }
        public bool Warn { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ChunkName);
            parameterList.Add(Warn);
        }
    }
}