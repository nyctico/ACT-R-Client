namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModChunk : AbstractEvaluationRequest
    {
        public ModChunk(string chunkName, object[] mods, string model = null) : base(
            "mod-chunk", model)
        {
            ChunkName = chunkName;
            Mods = mods;
        }

        public string ChunkName { get; set; }
        public object[] Mods { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(Mods);

            return list.ToArray();
        }
    }
}