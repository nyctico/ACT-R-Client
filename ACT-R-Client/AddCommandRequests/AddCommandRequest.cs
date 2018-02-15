using System;

namespace Nyctico.Actr.Client.AddCommandRequests
{
    public class AddCommandRequest : AbstractAddCommandRequest
    {
        private readonly Action<object[]> _execFunc;

        public AddCommandRequest(Action<object[]> execFunc, string publishedName, string privateName,
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