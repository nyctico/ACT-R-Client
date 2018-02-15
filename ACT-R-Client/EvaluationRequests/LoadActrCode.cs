namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class LoadActrCode : AbstractEvaluationRequest
    {
        public LoadActrCode(string path, string model = null) : base("load-act-r-code",
            model)
        {
            Path = path;
        }

        public string Path { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Path);

            return list.ToArray();
        }
    }
}