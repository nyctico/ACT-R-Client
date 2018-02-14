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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(SlotName);
            list.Add(NewValue);

            return list.ToArray();
        }
    }
}