using System;

namespace Nyctico.Actr.Client.AddCommandRequests
{
    public class AddCommandRequest : AbstractAddCommandRequest
    {
        private readonly Action<dynamic[]> _execFunc;

        public AddCommandRequest(Action<dynamic[]> execFunc, string publishedName, string privateName,
            string documentation, string multipleInstanceErrorMessage = null, string lispCmd = null) : base(
            publishedName, privateName, documentation, multipleInstanceErrorMessage, lispCmd)
        {
            _execFunc = execFunc;
        }

        public override void Execute(dynamic[] parameters)
        {
            _execFunc(parameters);
        }
    }
}