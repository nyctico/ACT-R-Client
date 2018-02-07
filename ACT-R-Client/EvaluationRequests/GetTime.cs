using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetTime : AbstractEvaluationRequest
    {
        public GetTime(bool modelTime = true, bool useModel = false, string model = null) : base("get-time", useModel,
            model)
        {
            ModelTime = modelTime;
        }

        public bool ModelTime { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ModelTime);

            return list;
        }
    }
}