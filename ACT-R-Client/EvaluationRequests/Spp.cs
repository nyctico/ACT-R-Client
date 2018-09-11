using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Spp : AbstractEvaluationRequest
    {
        public Spp(dynamic[] parameters, string model = null) : base("spp",
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