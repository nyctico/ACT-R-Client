using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetChunkSlotValue : AbstractEvaluationRequest
    {
        public SetChunkSlotValue(string chunkName, string slotName, object newValue,
            string model = null) : base("set-chunk-slot-value", model)
        {
            ChunkName = chunkName;
            SlotName = slotName;
            NewValue = newValue;
        }

        public string ChunkName { get; set; }
        public string SlotName { get; set; }
        public object NewValue { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ChunkName);
            parameterList.Add(SlotName);
            parameterList.Add(NewValue);
        }
    }
}