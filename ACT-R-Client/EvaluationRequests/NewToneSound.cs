using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class NewToneSound : AbstractEvaluationRequest
    {
        public NewToneSound(int frequence, double duration, double? onset = null, bool timeInMs = false,
            string model = null) : base("new-tone-sound", model)
        {
            Frequence = frequence;
            Duration = duration;
            Onset = onset;
            TimeInMs = timeInMs;
        }

        public int Frequence { set; get; }
        public double Duration { set; get; }
        public double? Onset { get; set; }
        public bool TimeInMs { set; get; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Frequence);
            list.Add(Duration);
            list.Add(Onset);
            list.Add(TimeInMs);

            return list;
        }
    }
}