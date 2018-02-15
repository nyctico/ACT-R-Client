using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ClearExpWindow : AbstractEvaluationRequest
    {
        public ClearExpWindow(Window window = null, string model = null) : base(
            "clear-exp-window", model)
        {
            Window = window;
        }

        public Window Window { set; get; }

        public override object[] ToParameterArray()
        {
            var list = BaseParameterList();

            list.Add(Window);

            return list.ToArray();
        }
    }
}