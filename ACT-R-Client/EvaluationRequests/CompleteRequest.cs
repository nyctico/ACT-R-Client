using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteRequest : AbstractEvaluationRequest
    {
        public CompleteRequest(string specId, bool useModel = false, string model = null) : base("complete-request",
            useModel,
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