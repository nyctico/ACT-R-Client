﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpTime : AbstractEvaluationRequest
    {
        public MpTime(string model = null) : base("mp-time", model)
        {
        }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();
            return list;
        }
    }
}