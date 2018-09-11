using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModChunk : AbstractEvaluationRequest
    {
        public ModChunk(string chunkName, dynamic[] mods, string model = null) : base(
            "mod-chunk", model)
        {
            ChunkName = chunkName;
            Mods = mods;
        }

        public string ChunkName { get; set; }
        public dynamic[] Mods { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ChunkName);
            parameterList.Add(Mods);
        }
    }
}