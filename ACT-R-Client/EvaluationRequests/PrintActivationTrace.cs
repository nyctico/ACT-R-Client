using System.Collections.Generic;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class PrintActivationTrace : AbstractEvaluationRequest
    {
        public PrintActivationTrace(int time, bool ms = true, bool useModel = false, string model = null) : base(
            "print-activation-trace", useModel,
            model)
        {
            Time = time;
            Ms = ms;
        }

        public int Time { set; get; }
        public bool Ms { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Time);
            list.Add(Ms);

            return list;
        }
    }
}