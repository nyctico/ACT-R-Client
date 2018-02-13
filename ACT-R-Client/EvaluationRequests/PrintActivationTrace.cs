using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintActivationTrace : AbstractEvaluationRequest
    {
        public PrintActivationTrace(int time, bool ms = true, string model = null) : base(
            "print-activation-trace",
            model)
        {
            Time = time;
            Ms = ms;
        }

        public int Time { set; get; }
        public bool Ms { set; get; }

        public override List<object> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(Ms);

            return list;
        }
    }
}