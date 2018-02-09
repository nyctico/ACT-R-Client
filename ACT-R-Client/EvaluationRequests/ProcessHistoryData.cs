using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ProcessHistoryData : AbstractEvaluationRequest
    {
        public ProcessHistoryData(string processor, bool file, List<dynamic> dataParameters,
            List<dynamic> processorParameters, string model = null) : base(
            "process-history-data",
            model)
        {
            Processor = Processor;
            File = file;
            DataParameters = dataParameters;
            ProcessorParameters = processorParameters;
        }

        public string Processor { get; set; }
        public bool File { get; set; }
        public List<dynamic> DataParameters { get; set; }
        public List<dynamic> ProcessorParameters { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Processor);
            list.Add(File);
            list.Add(DataParameters);
            list.Add(ProcessorParameters);

            return list;
        }
    }
}