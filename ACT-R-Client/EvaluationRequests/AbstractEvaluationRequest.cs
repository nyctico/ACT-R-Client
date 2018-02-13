using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public abstract class AbstractEvaluationRequest
    {
        public AbstractEvaluationRequest(string command, string model = null)
        {
            Command = command;
            Model = model;
        }

        public string Command { set; get; }
        public bool UseModel { set; get; }
        public string Model { set; get; }

        protected List<object> BaseParameterList()
        {
            var list = new List<object> {Command};

            if (Model == null) list.Add(false);
            else list.Add(Model);

            return list;
        }

        public abstract List<object> ToParameterList();
    }
}