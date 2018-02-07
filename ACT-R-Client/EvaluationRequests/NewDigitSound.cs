using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class NewDigitSound: AbstractEvaluationRequest
    {
        public NewDigitSound(long digit, double? onset = null, bool timeInMs = false,
            bool useModel = false, string model = null) : base("new-digit-sound", useModel, model)
        {
            Digit = digit;
            Onset = onset;
            TimeInMs = timeInMs;
        }

        public long Digit { set; get; }
        public double? Onset { get; set; }
        public bool TimeInMs { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Digit);
            list.Add(Onset);
            list.Add(TimeInMs);

            return list;
        }
    }
}