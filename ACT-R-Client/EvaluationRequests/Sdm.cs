using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdm : AbstractEvaluationRequest
    {
        public Sdm(List<object> specifications, string model = null) : base("sdm",
            model)
        {
            Specifications = specifications;
        }

        public List<object> Specifications { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Specifications);

            return list;
        }
    }
}