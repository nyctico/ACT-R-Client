using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class PermuteList : AbstractEvalCommand
    {
        public PermuteList(List<int> indexes, bool useModel = false, string model = null) : base("permute-list",
            useModel, model)
        {
            Indexes = indexes;
        }

        public List<int> Indexes { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Indexes);

            return list;
        }
    }
}