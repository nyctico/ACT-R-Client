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

        public dynamic[] Parameterlist
        {
            get
            {
                var parameterList = BaseParameterList();
                AddParameterToList(parameterList);
                return parameterList.ToArray();
            }
        }

        protected List<dynamic> BaseParameterList()
        {
            var list = new List<dynamic> {Command};

            if (Model == null) list.Add(false);
            else list.Add(Model);

            return list;
        }

        public abstract void AddParameterToList(List<dynamic> parameterList);
    }
}