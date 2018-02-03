using System;
using System.Collections.Generic;

namespace Nyctico.Actr.Client.Commands
{
    public class LambdaCommand : AbstractCommand
    {
        public Action<List<dynamic>> ExecFunc { set; get; }
        public LambdaCommand(string publishedName, string privateName, string documentation)
        {
            PublishedName = publishedName;
            PrivateName = privateName;
            Documentation = documentation;
        }

        public override void Execute(List<dynamic> parameters)
        {
            ExecFunc(parameters);
        }
    }
}