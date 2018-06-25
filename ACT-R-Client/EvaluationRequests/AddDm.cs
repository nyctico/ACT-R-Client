using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddDm : AbstractEvaluationRequest
    {
        public AddDm(object[] chunks, string model = null) : base("add-dm", model)
        {
            Chunks = chunks;
        }

        public object[] Chunks { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Chunks);
        }
    }
}