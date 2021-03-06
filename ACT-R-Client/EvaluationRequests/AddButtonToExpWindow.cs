﻿using System.Collections.Generic;
using Nyctico.Actr.Client.Data;

namespace Nyctico.Actr.Client.EvaluationRequests
{
    public class AddButtonToExpWindow : AbstractEvaluationRequest
    {
        public AddButtonToExpWindow(Window window, string text, int x, int y, dynamic[] action = null,
            int height = 50,
            int width = 75,
            string color = "gray", string model = null) : base("add-button-to-exp-window",
            model)
        {
            Window = window;
            Text = text;
            X = x;
            Y = y;
            Action = action;
            Height = height;
            Width = width;
            Color = color;
        }

        public Window Window { set; get; }
        public string Text { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public dynamic[] Action { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public string Color { set; get; }

        public override void AddParameterToList(List<dynamic> parameterList)
        {
            parameterList.Add(Window.ToJsonList());
            parameterList.Add(Text);
            parameterList.Add(X);
            parameterList.Add(Y);
            parameterList.Add(Action);
            parameterList.Add(Height);
            parameterList.Add(Width);
            parameterList.Add(Color);
        }
    }
}