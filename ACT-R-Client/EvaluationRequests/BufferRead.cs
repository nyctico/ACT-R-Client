using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class BufferRead : AbstractEvaluationRequest
    {
        public BufferRead(string bufferName = null, string model = null) : base("buffer-read",
            model)
        {
            BufferName = bufferName;
        }

        public string BufferName { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(BufferName);
        }
    }
}