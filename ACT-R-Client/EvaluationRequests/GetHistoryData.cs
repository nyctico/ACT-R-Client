namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class GetHistoryData : AbstractEvaluationRequest
    {
        public GetHistoryData(string history, string fileName = null, object[] parameters = null,
            string model = null) :
            base("get-history-data",
                model)
        {
            History = history;
            FileName = fileName;
            Parameters = parameters;
        }

        public string History { get; set; }
        public string FileName { get; set; }
        public object[] Parameters { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(History);
            list.Add(FileName);
            list.Add(Parameters);

            return list.ToArray();
        }
    }
}