namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Reload : AbstractEvaluationRequest
    {
        public Reload(bool compile = false) : base("reload")
        {
            Compile = compile;
        }

        public bool Compile { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Compile);

            return list.ToArray();
        }
    }
}