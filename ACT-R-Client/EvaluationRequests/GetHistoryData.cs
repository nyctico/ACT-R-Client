using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetHistoryData: AbstractEvaluationRequest
    {
        public GetHistoryData(string history, List<dynamic> parameters, bool useModel = false, string model = null) : base("get-history-data", useModel,
            model)
        {
            History = history;
            Parameters = parameters;
        }

        public string History { get; set; }
        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(History);
            list.Add(Parameters);

            return list;
        }
    }
}