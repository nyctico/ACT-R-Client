using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class Correlation : AbstractDispatcherEvaluate
    {
        public Correlation(List<dynamic> results, List<dynamic> data, bool output = true, bool useModel = false,
            string model = null) : base("correlation", useModel, model)
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