using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RecordHistory: AbstractEvaluationRequest
    {
        public RecordHistory(List<dynamic> parameters, bool useModel = false, string model = null) : base("record-history", useModel,
            model)
        {
            Parameters = parameters;
        }

        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Parameters);

            return list;
        }
    }
}