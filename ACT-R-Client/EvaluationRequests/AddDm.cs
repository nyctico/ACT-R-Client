using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddDm : AbstractEvaluationRequest
    {
        public AddDm(dynamic[] chunks, string model = null) : base("add-dm", model)
        {
            Chunks = chunks;
        }

        public dynamic[] Chunks { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Chunks);
        }
    }
}