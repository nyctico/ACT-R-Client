using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MeanDeviation : AbstractEvaluationRequest
    {
        public MeanDeviation(List<dynamic> results, List<dynamic> data, bool output = true,
            string model = null) : base("mean-deviation", model)
        {
            Results = results;
            Data = data;
            Output = output;
        }

        public List<dynamic> Results { set; get; }
        public List<dynamic> Data { set; get; }
        public bool Output { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Results);
            list.Add(Data);
            list.Add(Output);

            return list;
        }
    }
}