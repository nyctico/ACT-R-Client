using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class ModifyLineForExpWindow : AbstractEvaluationRequest
    {
        public ModifyLineForExpWindow(Line line, int[] start, int[] end, string color,
            string model = null) : base("modify-line-for-exp-window", model)
        {
            Line = line;
            Start = start;
            End = end;
            Color = color;
        }

        public Line Line { set; get; }
        public int[] Start { set; get; }
        public int[] End { set; get; }
        public string Color { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Line.ToJsonList());
            list.Add(Start);
            list.Add(End);
            if (Color != null) list.Add(Color);

            return list;
        }
    }
}