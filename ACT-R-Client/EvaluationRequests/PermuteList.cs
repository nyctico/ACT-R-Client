using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PermuteList : AbstractEvaluationRequest
    {
        public PermuteList(List<dynamic> indexes, string model = null) : base("permute-list",
            model)
        {
            Indexes = indexes;
        }

        public List<dynamic> Indexes { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Indexes);

            return list;
        }
    }
}