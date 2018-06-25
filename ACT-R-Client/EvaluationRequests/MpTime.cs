using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTime : AbstractEvaluationRequest
    {
        public MpTime(string model = null) : base("mp-time", model)
        {
        }

        public override void AddParameterToList(List<object> parameterList){}
    }
}