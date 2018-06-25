using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdp : AbstractEvaluationRequest
    {
        public Sdp(object[] parameters, string model = null) : base("sdp",
            model)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Parameters);
        }
    }
}