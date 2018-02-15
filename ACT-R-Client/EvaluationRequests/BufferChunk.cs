﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class BufferChunk : AbstractEvaluationRequest
    {
        public BufferChunk(List<string> bufferNames, string model = null) : base("buffer-chunk",
            model)
        {
            BufferNames = bufferNames;
        }

        public List<string> BufferNames { get; set; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(BufferNames);

            return list.ToArray();
        }
    }
}