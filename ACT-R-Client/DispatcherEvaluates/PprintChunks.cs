using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class PprintChunks: AbstractDispatcherEvaluate
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