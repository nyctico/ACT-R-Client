namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class Reload : AbstractEvaluationRequest
    {
        public Reload(bool compile = false) : base("reload")
        {
            Compile = compile;
        }

        public bool Compile { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Compile);

            return list.ToArray();
        }
    }
}