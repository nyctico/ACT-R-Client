using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class WhynotDm : AbstractEvaluationRequest
    {
        public WhynotDm(List<string> chunkNames, string model = null) : base("whynot-dm",
            model)
        {
            ChunkNames = chunkNames;
        }

        public List<string> ChunkNames { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkNames);

            return list;
        }
    }
}