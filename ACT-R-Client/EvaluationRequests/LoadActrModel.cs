using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class LoadActrModel : AbstractEvaluationRequest
    {
        public LoadActrModel(string path, string model = null) : base("load-act-r-model",
            model)
        {
            Path = path;
        }

        public string Path { get; set; }

        public override void AddParameterToList(List<object> parameterList)
        {
            parameterList.Add(Path);
        }
    }
}