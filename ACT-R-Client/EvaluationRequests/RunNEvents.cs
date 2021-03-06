﻿using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class RunNEvents : AbstractEvaluationRequest
    {
        public RunNEvents(long eventCount, bool realTime = false, string model = null) : base(
            "run-n-events", model)
        {
            EventCount = eventCount;
            RealTime = realTime;
        }

        public long EventCount { set; get; }
        public bool RealTime { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(EventCount);
            parameterList.Add(RealTime);
        }
    }
}