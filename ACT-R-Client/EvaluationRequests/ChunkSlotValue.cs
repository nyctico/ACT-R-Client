using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkSlotValue : AbstractEvaluationRequest
    {
        public ChunkSlotValue(string chunkName, string slotName, string model = null) : base(
            "chunk-slot-value", model)
        {
            ChunkName = chunkName;
            SlotName = slotName;
        }

        public string ChunkName { get; set; }
        public string SlotName { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(SlotName);

            return list;
        }
    }
}