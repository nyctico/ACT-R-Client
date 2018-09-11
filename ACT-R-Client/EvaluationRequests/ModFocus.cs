using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModFocus : AbstractEvaluationRequest
    {
        public ModFocus(dynamic[] mods, string model = null) : base("mod-focus",
            model)
        {
            Mods = mods;
        }

        public dynamic[] Mods { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Mods);
        }
    }
}