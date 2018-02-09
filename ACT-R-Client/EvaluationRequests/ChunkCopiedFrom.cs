﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ChunkCopiedFrom : AbstractEvaluationRequest
    {
        public ChunkCopiedFrom(string chunkName,
            string model = null) : base("chunk-copied-from", model)
        {
            ChunkName = chunkName;
        }

        public string ChunkName { get; set; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(ChunkName);

            return list;
        }
    }
}