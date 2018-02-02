using System.Collections.Generic;

namespace Nyctico.Actr.Client.Abstracts
{
    public abstract class AbstractEvalCommand
    {
        public string Command { set; get; }
        public bool UseModel { set; get; }
        public string Model { set; get; }

        public AbstractEvalCommand(string command, bool useModel=false, string model=null)
        {
            Command = command;
            UseModel = useModel;
            Model = model;
        }
        
        protected List<dynamic> BaseParameterList()
        {
            List<dynamic> list = new List<dynamic> {Command};

            if (!UseModel) list.Add(UseModel); else list.Add(Model);

            return list;
        }

        public abstract List<dynamic> ToParameterList();
    }
}