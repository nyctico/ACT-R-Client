using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class StopRecordingHistory : AbstractEvaluationRequest
    {
        public StopRecordingHistory(List<dynamic> parameters, string model = null) : base(
            "stop-recording-history",
            model)
        {
            Parameters = parameters;
        }

        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}