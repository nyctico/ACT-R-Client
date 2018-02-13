using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PermuteList : AbstractEvaluationRequest
    {
        public PermuteList(List<object> indexes, string model = null) : base("permute-list",
            model)
        {
            Indexes = indexes;
        }

        public List<object> Indexes { set; get; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Indexes);

            return list;
        }
    }
}