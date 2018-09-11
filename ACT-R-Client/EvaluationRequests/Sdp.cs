using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Sdp : AbstractEvaluationRequest
    {
        public Sdp(dynamic[] parameters, string model = null) : base("sdp",
            model)
        {
            Parameters = parameters;
        }

        public dynamic[] Parameters { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Parameters);
        }
    }
}