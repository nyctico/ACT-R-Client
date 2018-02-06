using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class AddDm: AbstractDispatcherEvaluate
    {
        public List<dynamic> Chunks { get; set; }

        public AddDm(List<dynamic> chunks, bool useModel = false, string model = null) : base("add-dm", useModel, model)
        {
            Chunks = chunks;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Chunks);

            return list;
        }
    }
}