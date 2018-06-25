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

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Value);
        }
    }
}