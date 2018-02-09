using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SaveHistoryData : AbstractEvaluationRequest
    {
        public SaveHistoryData(string history, bool file, string comments, List<dynamic> parameters,
            string model = null) : base("save-history-datae",
            model)
        {
            History = history;
            File = file;
            Comments = comments;
            Parameters = parameters;
        }

        public string History { get; set; }
        public bool File { get; set; }
        public string Comments { get; set; }
        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(History);
            list.Add(File);
            list.Add(Comments);
            list.Add(Parameters);

            return list;
        }
    }
}