using System.Collections.Generic;

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

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Mods);
        }
    }
}