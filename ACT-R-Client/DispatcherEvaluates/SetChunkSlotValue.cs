using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class SetChunkSlotValue: AbstractDispatcherEvaluate
    {
        public string ChunkName { get; set; }
        public string SlotName { get; set; }
        public string NewValue { get; set; }

        public SetChunkSlotValue(string chunkName, string slotName, string newValue, bool useModel = false, string model = null) : base("set-chunk-slot-value", useModel, model)
        {
            ChunkName = chunkName;
            SlotName = slotName;
            NewValue = newValue;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(SlotName);
            list.Add(NewValue);

            return list;
        }
    }
}