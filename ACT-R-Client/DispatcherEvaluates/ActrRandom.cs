using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class ActrRandom : AbstractDispatcherEvaluate
    {
        public ActrRandom(long value, bool useModel = false, string model = null) : base("act-r-random", useModel,
            model)
        {
            Value = value;
        }

        public long Value { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Value);

            return list;
        }
    }
}