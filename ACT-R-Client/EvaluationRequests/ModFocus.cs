using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModFocus: AbstractEvaluationRequest
    {
        public List<dynamic> Mods { get; set; }

        public ModFocus(List<dynamic> mods, bool useModel = false, string model = null) : base("mod-focus", useModel, model)
        {
            Mods = mods;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Mods);

            return list;
        }
    }
}