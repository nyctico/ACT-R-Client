using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Correlation : AbstractEvaluationRequest
    {
        public Correlation(List<double> results, List<double> data, bool output = true,
            string model = null) : base("correlation", model)
        {
            Results = results;
            Data = data;
            Output = output;
        }

        public List<double> Results { set; get; }
        public List<double> Data { set; get; }
        public bool Output { set; get; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Results);
            parameterList.Add(Data);
            parameterList.Add(Output);
        }
    }
}