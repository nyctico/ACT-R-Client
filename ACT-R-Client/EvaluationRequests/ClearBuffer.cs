using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ClearBuffer : AbstractEvaluationRequest
    {
        public ClearBuffer(string buffer = null, string model = null) : base("clear-buffer",
            model)
        {
            Buffer = buffer;
        }

        public string Buffer { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);

            return list;
        }
    }
}