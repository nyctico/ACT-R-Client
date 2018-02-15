namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class NewDigitSound : AbstractEvaluationRequest
    {
        public NewDigitSound(long digit, double? onset = null, bool timeInMs = false,
            string model = null) : base("new-digit-sound", model)
        {
            Digit = digit;
            Onset = onset;
            TimeInMs = timeInMs;
        }

        public long Digit { set; get; }
        public double? Onset { get; set; }
        public bool TimeInMs { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Digit);
            list.Add(Onset);
            list.Add(TimeInMs);

            return list.ToArray();
        }
    }
}