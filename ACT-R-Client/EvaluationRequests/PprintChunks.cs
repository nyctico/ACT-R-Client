using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PprintChunks : AbstractEvaluationRequest
    {
        public PprintChunks(List<string> chunkNames, string model = null) : base("pprint-chunks",
            model)
        {
            ChunkNames = chunkNames;
        }

        public List<string> ChunkNames { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkNames);

            return list.ToArray();
        }
    }
}