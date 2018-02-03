using System;
using System.Collections.Generic;

namespace Nyctico.Actr.Client.Commands
{
    public class LambdaCommand : AbstractCommand
    {
        private Action<List<dynamic>> _execFunc;

        public LambdaCommand(Action<List<dynamic>> execFunc, string publishedName, string privateName, string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(List<dynamic> parameters)
        {
            _execFunc(parameters);
        }
    }
}