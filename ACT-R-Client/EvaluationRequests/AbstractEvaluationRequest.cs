using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public abstract class AbstractEvaluationRequest
    {
        public AbstractEvaluationRequest(string command, bool useModel = false, string model = null)
        {
            Command = command;
            UseModel = useModel;
            Model = model;
        }

        public string Command { set; get; }
        public bool UseModel { set; get; }
        public string Model { set; get; }

        protected List<dynamic> BaseParameterList()
        {
            var list = new List<dynamic> {Command};

            if (!UseModel) list.Add(UseModel);
            else list.Add(Model);

            return list;
        }

        public abstract List<dynamic> ToParameterList();
    }
}