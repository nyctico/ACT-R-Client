using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetHistoryData : AbstractEvaluationRequest
    {
        public GetHistoryData(string history, string fileName = null, object[] parameters = null,
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
        public object[] Parameters { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(History);
            parameterList.Add(FileName);
            parameterList.Add(Parameters);
        }
    }
}