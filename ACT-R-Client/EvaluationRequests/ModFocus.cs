using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModFocus : AbstractEvaluationRequest
    {
        public ModFocus(List<dynamic> mods, string model = null) : base("mod-focus",
            model)
        {
            Mods = mods;
        }

        public List<dynamic> Mods { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Mods);

            return list;
        }
    }
}