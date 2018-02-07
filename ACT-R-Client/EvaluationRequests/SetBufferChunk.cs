using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetBufferChunk: AbstractEvaluationRequest
    {
        public string BufferName { get; set; }
        public string ChunkName { get; set; }
        public bool Requested { get; set; }

        public SetBufferChunk(string bufferName, string chunkName, bool requested=true, bool useModel = false, string model = null) : base("set-buffer-chunk", useModel, model)
        {
            BufferName = bufferName;
            ChunkName = chunkName;
            Requested = requested;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(BufferName);
            list.Add(ChunkName);
            list.Add(Requested);

            return list;
        }
    }
}