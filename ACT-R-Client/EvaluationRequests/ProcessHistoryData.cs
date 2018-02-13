using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ProcessHistoryData : AbstractEvaluationRequest
    {
        public ProcessHistoryData(string processor, string fileName = null, List<object> parameters = null,
            string model = null) : base(
            "process-history-data",
            model)
        {
            Processor = processor;
            FileName = fileName;
            Parameters = parameters;
        }

        public string Processor { get; set; }
        public string FileName { get; set; }
        public List<object> Parameters { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Processor);
            list.Add(FileName);
            list.Add(Parameters);

            return list;
        }
    }
}