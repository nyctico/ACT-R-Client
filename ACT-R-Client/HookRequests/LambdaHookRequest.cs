﻿using System;
using System.Collections.Generic;

namespace Nyctico.Actr.Client.HookRequests
{
    public class LambdaHookRequest : AbstractHookRequest
    {
        private readonly Action<List<dynamic>> _execFunc;

        public LambdaHookRequest(Action<List<dynamic>> execFunc, string publishedName, string privateName,
            string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(
            publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(List<dynamic> parameters)
        {
            _execFunc(parameters);
        }
    }
}