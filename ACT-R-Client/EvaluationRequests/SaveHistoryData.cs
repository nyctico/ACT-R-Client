using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SaveHistoryData : AbstractEvaluationRequest
    {
        public SaveHistoryData(string historyName, string fileName, dynamic[] parameters,
            string model = null) : base("save-history-data",
            model)
        {
            HistoryName = historyName;
            FileName = fileName;
            Parameters = parameters;
        }

        public string HistoryName { get; set; }
        public string FileName { get; set; }
        public dynamic[] Parameters { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(HistoryName);
            parameterList.Add(FileName);
            parameterList.Add(Parameters);
        }
    }
}