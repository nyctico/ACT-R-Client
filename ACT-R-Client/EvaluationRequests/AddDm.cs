namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddDm : AbstractEvaluationRequest
    {
        public AddDm(object[] chunks, string model = null) : base("add-dm", model)
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