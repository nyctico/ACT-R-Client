using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModFocus : AbstractEvaluationRequest
    {
        public ModFocus(List<object> mods, string model = null) : base("mod-focus",
            model)
        {
            Mods = mods;
        }

        public List<object> Mods { get; set; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Mods);

            return list;
        }
    }
}