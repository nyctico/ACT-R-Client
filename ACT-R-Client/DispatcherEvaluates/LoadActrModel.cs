﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class LoadActrModel: AbstractDispatcherEvaluate
    {
        public string Path { get; set; }

        public LoadActrModel(string path, bool useModel = false, string model = null) : base("load-act-r-model", useModel, model)
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