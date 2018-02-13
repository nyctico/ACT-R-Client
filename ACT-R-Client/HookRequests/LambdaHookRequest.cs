using System;
using System.Collections.Generic;

namespace Nyctico.Actr.Client.HookRequests
{
    public class LambdaHookRequest : AbstractHookRequest
    {
        private readonly Action<List<object>> _execFunc;

        public LambdaHookRequest(Action<List<object>> execFunc, string publishedName, string privateName,
            string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(
            publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(List<object> parameters)
        {
            _execFunc(parameters);
        }
    }
}