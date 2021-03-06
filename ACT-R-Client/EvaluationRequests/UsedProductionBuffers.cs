﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class UsedProductionBuffers : AbstractEvaluationRequest
    {
        public UsedProductionBuffers(string model = null) : base("used-production-buffers",
            model)
        {
        }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
        }
    }
}