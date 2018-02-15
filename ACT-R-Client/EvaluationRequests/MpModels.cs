﻿namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class MpModels : AbstractEvaluationRequest
    {
        public MpModels(string model = null) : base("mp-models", model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}