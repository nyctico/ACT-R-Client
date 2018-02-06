using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class BufferChunk : AbstractDispatcherEvaluate
    {
        public List<dynamic> Parameters { get; set; }

        public BufferChunk(List<dynamic> parameters, bool useModel = false, string model = null) : base("buffer-chunk", useModel, model)
        {
            Parameters = parameters;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}