using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class HistoryDataAvailable : AbstractEvaluationRequest
    {
        public HistoryDataAvailable(string history, bool file, List<dynamic> parameters, bool useModel = false,
            string model = null) : base("history-data-available", useModel,
            model)
        {
            History = history;
            File = file;
            Parameters = parameters;
        }

        public string History { get; set; }
        public bool File { get; set; }
        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(History);
            list.Add(File);
            list.Add(Parameters);

            return list;
        }
    }
}