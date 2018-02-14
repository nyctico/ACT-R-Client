using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class BufferStatus : AbstractEvaluationRequest
    {
        public BufferStatus(List<string> bufferNames, string model = null) : base(
            "buffer-status", model)
        {
            BufferNames = bufferNames;
        }

        public List<string> BufferNames { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(BufferNames);

            return list.ToArray();
        }
    }
}