using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetTime : AbstractEvaluationRequest
    {
        public GetTime(bool modelTime = true, string model = null) : base("get-time",
            model)
        {
            ModelTime = modelTime;
        }

        public bool ModelTime { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(ModelTime);
        }
    }
}