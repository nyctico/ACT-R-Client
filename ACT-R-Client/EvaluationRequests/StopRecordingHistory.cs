using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class StopRecordingHistory : AbstractEvaluationRequest
    {
        public StopRecordingHistory(string historyName, string model = null) : base(
            "stop-recording-history",
            model)
        {
            HistoryName = historyName;
        }

        public string HistoryName { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(HistoryName);
        }
    }
}