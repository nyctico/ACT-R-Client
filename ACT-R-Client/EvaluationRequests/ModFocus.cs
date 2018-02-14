namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModFocus : AbstractEvaluationRequest
    {
        public ModFocus(object[] mods, string model = null) : base("mod-focus",
            model)
        {
            Mods = mods;
        }

        public object[] Mods { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Mods);

            return list.ToArray();
        }
    }
}