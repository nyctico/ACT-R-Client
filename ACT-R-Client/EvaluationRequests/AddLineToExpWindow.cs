﻿using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddLineToExpWindow : AbstractEvaluationRequest
    {
        public AddLineToExpWindow(Window window, int[] start, int[] end, string color = null, bool useModel = false,
            string model = null) : base("add-line-to-exp-window", useModel,
            model)
        {
            Window = window;
            Start = start;
            End = end;
            Color = color;
        }

        public Window Window { set; get; }
        public int[] Start { set; get; }
        public int[] End { set; get; }
        public string Color { set; get; }

        public override List<dynamic> ToParameterList()
        {
            var list = BaseParameterList();

            list.Add(Window.ToJsonList());
            list.Add(Start);
            list.Add(End);
            if (Color != null) list.Add(Color);

            return list;
        }
    }
}