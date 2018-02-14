namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PermuteList : AbstractEvaluationRequest
    {
        public PermuteList(object[] indexes, string model = null) : base("permute-list",
            model)
        {
            Indexes = indexes;
        }

        public object[] Indexes { set; get; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Indexes);

            return list.ToArray();
        }
    }
}