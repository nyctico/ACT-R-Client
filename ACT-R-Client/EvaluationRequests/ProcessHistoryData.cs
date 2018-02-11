using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ProcessHistoryData : AbstractEvaluationRequest
    {
        public ProcessHistoryData(string processor, string fileName = null, List<dynamic> parameters = null,
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
        public List<dynamic> Parameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Processor);
            list.Add(FileName);
            list.Add(Parameters);

            return list;
        }
    }
}