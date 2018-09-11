using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PermuteList : AbstractEvaluationRequest
    {
        public PermuteList(dynamic[] indexes, string model = null) : base("permute-list",
            model)
        {
            Indexes = indexes;
        }

        public dynamic[] Indexes { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Indexes);
        }
    }
}