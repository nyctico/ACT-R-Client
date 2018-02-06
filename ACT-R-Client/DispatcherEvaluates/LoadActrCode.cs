using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class LoadActrCode: AbstractDispatcherEvaluate
    {
        public string Path { get; set; }

        public LoadActrCode(string path, bool useModel = false, string model = null) : base("load-act-r-code", useModel, model)
        {
            Path = path;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Path);

            return list;
        }
    }
}