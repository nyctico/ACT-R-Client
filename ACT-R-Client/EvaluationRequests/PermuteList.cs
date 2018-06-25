using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PermuteList : AbstractEvaluationRequest
    {
        public PermuteList(object[] indexes, string model = null) : base("permute-list",
            model)
        {
            Indexes = indexes;
        }

        public object[] Indexes { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Indexes);
        }
    }
}