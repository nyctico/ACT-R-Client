using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ActrRandom : AbstractEvaluationRequest
    {
        public ActrRandom(long value, string model = null) : base("act-r-random",
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