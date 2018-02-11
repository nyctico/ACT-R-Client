﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class BufferRead : AbstractEvaluationRequest
    {
        public BufferRead(string bufferName = null, string model = null) : base("buffer-read",
            model)
        {
            BufferName = bufferName;
        }

        public string BufferName { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(BufferName);

            return list;
        }
    }
}