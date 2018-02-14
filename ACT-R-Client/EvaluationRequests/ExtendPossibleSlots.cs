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

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(Warn);

            return list.ToArray();
        }
    }
}