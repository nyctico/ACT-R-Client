using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PprintChunks: AbstractEvaluationRequest
    {
        public List<dynamic> Chunks { get; set; }

        public PprintChunks(List<dynamic> chunks, bool useModel = false, string model = null) : base("pprint-chunks", useModel, model)
        {
            Chunks = chunks;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list;
        }
    }
}