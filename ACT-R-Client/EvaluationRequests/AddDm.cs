using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddDm : AbstractEvaluationRequest
    {
        public AddDm(List<dynamic> chunks, string model = null) : base("add-dm", model)
        {
            Chunks = chunks;
        }

        public List<dynamic> Chunks { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list;
        }
    }
}