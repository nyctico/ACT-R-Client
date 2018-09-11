using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdm : AbstractEvaluationRequest
    {
        public Sdm(dynamic[] specifications, string model = null) : base("sdm",
            model)
        {
            Specifications = specifications;
        }

        public dynamic[] Specifications { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Specifications);
        }
    }
}