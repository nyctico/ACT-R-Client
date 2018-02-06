using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class DefineChunks: AbstractDispatcherEvaluate
    {
        public List<dynamic> Chunks { get; set; }

        public DefineChunks(List<dynamic> chunks, bool useModel = false, string model = null) : base("define-chunks", useModel, model)
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