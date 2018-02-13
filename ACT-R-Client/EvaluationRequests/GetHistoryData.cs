using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetHistoryData : AbstractEvaluationRequest
    {
        public GetHistoryData(string history, string fileName = null, List<object> parameters = null,
            string model = null) :
            base("get-history-data",
                model)
        {
            History = history;
            FileName = fileName;
            Parameters = parameters;
        }

        public string History { get; set; }
        public string FileName { get; set; }
        public List<object> Parameters { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(History);
            list.Add(FileName);
            list.Add(Parameters);

            return list;
        }
    }
}