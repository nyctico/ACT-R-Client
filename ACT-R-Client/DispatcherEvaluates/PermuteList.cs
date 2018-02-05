﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class PermuteList : AbstractDispatcherEvaluate
    {
        public PermuteList(List<dynamic> indexes, bool useModel = false, string model = null) : base("permute-list",
            useModel, model)
        {
            Indexes = indexes;
        }

        public List<dynamic> Indexes { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Indexes);

            return list;
        }
    }
}