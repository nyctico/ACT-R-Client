﻿namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class CompleteAllBufferRequests : AbstractEvaluationRequest
    {
        public CompleteAllBufferRequests(string bufferName,
            string model = null) : base("complete-all-buffer-requests", model)
        {
            BufferName = bufferName;
        }

        public string BufferName { get; set; }

        public override object[] ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(BufferName);

            return list.ToArray();
        }
    }
}