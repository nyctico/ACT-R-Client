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

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Buffer);
        }
    }
}