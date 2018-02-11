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

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(HistoryName);

            return list;
        }
    }
}