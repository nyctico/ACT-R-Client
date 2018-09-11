using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ProcessHistoryData : AbstractEvaluationRequest
    {
        public ProcessHistoryData(string processor, string fileName = null, dynamic[] parameters = null,
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
        public dynamic[] Parameters { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Processor);
            parameterList.Add(FileName);
            parameterList.Add(Parameters);
        }
    }
}