using System;
using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherHooks
{
    public class LambdaDispatcherHook : AbstractDispatcherHook
    {
        private Action<List<dynamic>> _execFunc;

        public LambdaDispatcherHook(Action<List<dynamic>> execFunc, string publishedName, string privateName, string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(List<dynamic> parameters)
        {
            _execFunc(parameters);
        }
    }
}