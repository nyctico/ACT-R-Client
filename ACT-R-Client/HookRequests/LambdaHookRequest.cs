using System;

namespace Nyctico.Actr.Client.HookRequests
{
    public class LambdaHookRequest : AbstractHookRequest
    {
        private readonly Action<object[]> _execFunc;

        public LambdaHookRequest(Action<object[]> execFunc, string publishedName, string privateName,
            string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(
            publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(object[] parameters)
        {
            _execFunc(parameters);
        }
    }
}