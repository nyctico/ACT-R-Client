using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Dm : AbstractEvaluationRequest
    {
        public Dm(List<string> chunkNames, string model = null) : base("dm",
            model)
        {
            ChunkNames = chunkNames;
        }

        public List<string> ChunkNames { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(ChunkNames);

            return list.ToArray();
        }
    }
}