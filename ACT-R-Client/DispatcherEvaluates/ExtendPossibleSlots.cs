using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class ExtendPossibleSlots: AbstractDispatcherEvaluate
    {
        public string ChunkName { get; set; }
        public bool Warn { get; set; }

        public ExtendPossibleSlots(string chunkName, bool warn=true, bool useModel = false, string model = null) : base("extend-possible-slots", useModel, model)
        {
            ChunkName = chunkName;
            Warn = warn;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);
            list.Add(Warn);

            return list;
        }
    }
}