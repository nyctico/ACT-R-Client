namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ProcessHistoryData : AbstractEvaluationRequest
    {
        public ProcessHistoryData(string processor, string fileName = null, object[] parameters = null,
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
        public object[] Parameters { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Processor);
            list.Add(FileName);
            list.Add(Parameters);

            return list.ToArray();
        }
    }
}