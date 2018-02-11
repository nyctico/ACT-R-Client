using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdm : AbstractEvaluationRequest
    {
        public Sdm(List<dynamic> specifications, string model = null) : base("sdm",
            model)
        {
            Specifications = specifications;
        }

        public List<dynamic> Specifications { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Specifications);

            return list;
        }
    }
}