using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ClearBuffer: AbstractEvaluationRequest
    {
        public string Buffer { get; set; }

        public ClearBuffer(string buffer=null, bool useModel = false, string model = null) : base("clear-buffer", useModel, model)
        {
            Buffer = buffer;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);

            return list;
        }
    }
}