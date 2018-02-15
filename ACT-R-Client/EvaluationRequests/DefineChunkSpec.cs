namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class DefineChunkSpec : AbstractEvaluationRequest
    {
        public DefineChunkSpec(object[] spec, string model = null) : base(
            "define-chunk-spec",
            model)
        {
            Spec = spec;
        }

        public object[] Spec { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Spec);

            return list.ToArray();
        }
    }
}