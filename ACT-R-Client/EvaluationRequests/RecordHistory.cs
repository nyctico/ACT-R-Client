using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RecordHistory : AbstractEvaluationRequest
    {
        public RecordHistory(string historyName, string model = null) : base(
            "record-history",
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