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

        public object[] Parameterlist
        {
            get
            {
                List<object> parameterList = BaseParameterList();
                AddParameterToList(parameterList);
                return parameterList.ToArray();
            }
        }

        protected List<object> BaseParameterList()
        {
            var list = new List<object> {Command};

            if (Model == null) list.Add(false);
            else list.Add(Model);

            return list;
        }

        public abstract void AddParameterToList(List<object> parameterList);
    }
}