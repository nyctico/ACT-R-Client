namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunks : AbstractEvaluationRequest
    {
        public DefineChunks(object[] chunks, string model = null) : base("define-chunks",
            model)
        {
            Chunks = chunks;
        }

        public object[] Chunks { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list.ToArray();
        }
    }
}