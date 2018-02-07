using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class BufferRead : AbstractEvaluationRequest
    {
        public BufferRead(string buffer = null, bool useModel = false, string model = null) : base("buffer-read",
            useModel, model)
        {
            Buffer = buffer;
        }

        public string Buffer { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);

            return list;
        }
    }
}