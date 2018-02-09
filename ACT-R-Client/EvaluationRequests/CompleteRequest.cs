using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteRequest : AbstractEvaluationRequest
    {
        public CompleteRequest(string specId, string model = null) : base("complete-request",
            model)
        {
            SpecId = specId;
        }

        public string SpecId { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(SpecId);

            return list;
        }
    }
}