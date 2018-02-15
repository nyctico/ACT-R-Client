﻿namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintBoldResponseData : AbstractEvaluationRequest
    {
        public PrintBoldResponseData(string model = null) : base("print-bold-response-data",
            model)
        {
        }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();
            return list.ToArray();
        }
    }
}