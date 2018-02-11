using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SaveHistoryData : AbstractEvaluationRequest
    {
        public SaveHistoryData(string historyName, string fileName, List<dynamic> parameters,
            string model = null) : base("save-history-data",
            model)
        {
            HistoryName = historyName;
            FileName = fileName;
            Parameters = parameters;
        }

        public string HistoryName { get; set; }
        public string FileName { get; set; }
        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(HistoryName);
            list.Add(FileName);
            list.Add(Parameters);

            return list;
        }
    }
}