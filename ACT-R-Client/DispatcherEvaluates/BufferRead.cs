using System.Collections.Generic;

namespace Nyctico.Actr.Client.DispatcherEvaluates
{
    public class BufferRead: AbstractDispatcherEvaluate
    {
        public string Buffer { get; set; }

        public BufferRead(string buffer=null, bool useModel = false, string model = null) : base("buffer-read", useModel, model)
        {
            Buffer = buffer;
        }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Buffer);

            return list;
        }
    }
}