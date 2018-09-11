using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class NewWordSound : AbstractEvaluationRequest
    {
        public NewWordSound(string word, double? onset = null, string location = "external", bool timeInMs = false,
            string model = null) : base("new-word-sound", model)
        {
            Word = word;
            Onset = onset;
            Location = location;
            TimeInMs = timeInMs;
        }

        public string Word { set; get; }
        public double? Onset { get; set; }
        public string Location { get; set; }
        public bool TimeInMs { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Word);
            parameterList.Add(Onset);
            parameterList.Add(Location);
            parameterList.Add(TimeInMs);
        }
    }
}