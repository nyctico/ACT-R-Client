using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SetBufferChunk : AbstractEvaluationRequest
    {
        public SetBufferChunk(string bufferName, string chunkName, bool requested = true,
            string model = null) : base("set-buffer-chunk", model)
        {
            BufferName = bufferName;
            ChunkName = chunkName;
            Requested = requested;
        }

        public string BufferName { get; set; }
        public string ChunkName { get; set; }
        public bool Requested { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(BufferName);
            parameterList.Add(ChunkName);
            parameterList.Add(Requested);
        }
    }
}