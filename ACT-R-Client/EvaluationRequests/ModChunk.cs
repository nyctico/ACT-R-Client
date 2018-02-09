using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModChunk : AbstractEvaluationRequest
    {
        public ModChunk(string chunkName, List<dynamic> mods, string model = null) : base(
            "mod-chunk", model)
        {
            ChunkName = chunkName;
            Mods = mods;
        }

        public string ChunkName { get; set; }
        public List<dynamic> Mods { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(Mods);

            return list;
        }
    }
}