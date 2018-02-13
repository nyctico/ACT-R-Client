using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddDm : AbstractEvaluationRequest
    {
        public AddDm(List<object> chunks, string model = null) : base("add-dm", model)
        {
            Chunks = chunks;
        }

        public List<object> Chunks { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list;
        }
    }
}