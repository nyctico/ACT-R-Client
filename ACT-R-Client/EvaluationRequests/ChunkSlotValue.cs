using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkSlotValue: AbstractEvaluationRequest
    {
        public string ChunkName { get; set; }
        public string SlotName { get; set; }

        public ChunkSlotValue(string chunkName, string slotName, bool useModel = false, string model = null) : base("chunk-slot-value", useModel, model)
        {
            ChunkName = chunkName;
            SlotName = slotName;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(SlotName);

            return list;
        }
    }
}