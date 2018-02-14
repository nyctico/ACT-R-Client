namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class SaveHistoryData : AbstractEvaluationRequest
    {
        public SaveHistoryData(string historyName, string fileName, object[] parameters,
            string model = null) : base("save-history-data",
            model)
        {
            HistoryName = historyName;
            FileName = fileName;
            Parameters = parameters;
        }

        public string HistoryName { get; set; }
        public string FileName { get; set; }
        public object[] Parameters { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(HistoryName);
            list.Add(FileName);
            list.Add(Parameters);

            return list.ToArray();
        }
    }
}