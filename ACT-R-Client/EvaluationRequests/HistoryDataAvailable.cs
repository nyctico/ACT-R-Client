using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class HistoryDataAvailable : AbstractEvaluationRequest
    {
        public HistoryDataAvailable(string history, bool file, dynamic[] parameters,
            string model = null) : base("history-data-available",
            model)
        {
            History = history;
            File = file;
            Parameters = parameters;
        }

        public string History { get; set; }
        public bool File { get; set; }
        public dynamic[] Parameters { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(History);
            parameterList.Add(File);
            parameterList.Add(Parameters);
        }
    }
}