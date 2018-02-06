using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class ModChunk: AbstractDispatcherEvaluate
    {
        public string ChunkName { get; set; }
        public List<dynamic> Mods { get; set; }

        public ModChunk(string chunkName, List<dynamic> mods, bool useModel = false, string model = null) : base("mod-chunk", useModel, model)
        {
            ChunkName = chunkName;
            Mods = mods;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(Mods);

            return list;
        }
    }
}