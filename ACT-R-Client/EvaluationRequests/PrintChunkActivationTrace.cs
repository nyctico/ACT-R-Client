﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintChunkActivationTrace : AbstractEvaluationRequest
    {
        public PrintChunkActivationTrace(string chunkName, int time, bool ms = true,
            string model = null) : base("print-chunk-activation-trace", model)
        {
            ChunkName = chunkName;
            Time = time;
            Ms = ms;
        }

        public string ChunkName { get; set; }
        public int Time { get; set; }
        public bool Ms { get; set; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(ChunkName);
            parameterList.Add(Time);
            parameterList.Add(Ms);
        }
    }
}