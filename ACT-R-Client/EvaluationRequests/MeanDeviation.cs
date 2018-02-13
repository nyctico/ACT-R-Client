using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MeanDeviation : AbstractEvaluationRequest
    {
        public MeanDeviation(List<double> results, List<double> data, bool output = true,
            string model = null) : base("mean-deviation", model)
        {
            Results = results;
            Data = data;
            Output = output;
        }

        public List<double> Results { set; get; }
        public List<double> Data { set; get; }
        public bool Output { set; get; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Results);
            list.Add(Data);
            list.Add(Output);

            return list;
        }
    }
}