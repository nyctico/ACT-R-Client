﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.AddCommandRequests;
using Nyctico.Actr.Client.MonitorRequests;

namespace Nyctico.Actr.Example.Tutorials
{
    public static class Sperling
    {
        private static List<string> _answers;
        private static List<string> _responses;
        private static readonly List<double> Results = new List<double> {0.0, 0.0, 0.0, 0.0};
        private static readonly List<double> Onsets = new List<double> {0.0, .15, .3, 1.0};

        public static void Execute(string ip, int port, bool realTime, int numberOfRuns)
        {
            using (var actr = new ActRClient(ip, port))
            {
                actr.StartTraceMonitoring();
                actr.LoadActrModel("ACT-R:tutorial;unit3;sperling-model.lisp");
                actr.Reset();

                for (var i = 0; i < numberOfRuns; ++i) OneBlock(actr, realTime);

                var expData = new List<double> {3.03, 2.4, 2.03, 1.5};
                for (var i = 0; i < Results.Count; ++i) Results[i] = Results[i] / numberOfRuns;

                actr.Correlation(Results, expData);
                actr.MeanDeviation(Results, expData);

                Console.WriteLine("Condition    Current Participant   Original Experiment");
                for (var i = 0; i < Results.Count; ++i)
                    Console.WriteLine(
                        $" {Onsets[i]:N2} sec.            {Results[i]:N2}                  {expData[i]:N2}");
            }
        }

        private static void OneBlock(ActRClient actr, bool runInRealTime)
        {
            var result = new List<double>();
            var permuteList = actr.PermuteList(Onsets.Select<double, object>(i => i).ToArray());

            foreach (double r in permuteList) result.Add(Trial(actr, runInRealTime, r));
            result.Sort((d, d1) => d1.CompareTo(d));
            for (var i = 0; i < Results.Count; ++i) Results[i] = Results[i] + result[i];
        }

        private static double Trial(ActRClient actr, bool runInRealTime, double onset)
        {
            _answers = new List<string>();
            _responses = new List<string>();

            actr.Reset();

            int freq;

            var row = actr.ActrRandom(3);

            var window = actr.OpenExpWindow("Sperling Experiment", runInRealTime);

            var letters = new dynamic[]
            {
                "B",
                "C",
                "D",
                "F",
                "G",
                "H",
                "J",
                "K",
                "L",
                "M",
                "N",
                "P",
                "Q",
                "R",
                "S",
                "T",
                "V",
                "W",
                "X",
                "Y",
                "Z"
            };
            letters = actr.PermuteList(letters);
            for (var i = 0; i < 3; ++i)
            for (var j = 0; j < 4; ++j)
            {
                var txt = letters[i + j * 4];
                if (i == row)
                    _answers.Add((string) txt);
                actr.AddTextToWindow(window, (string) txt, 75 + j * 50, 101 + i * 50);
            }

            actr.InstallDevice(window);
            switch (row)
            {
                case 0:
                    freq = 2000;
                    break;
                case 1:
                    freq = 1000;
                    break;
                default:
                    freq = 500;
                    break;
            }

            actr.NewToneSound(freq, 0.5, onset);
            actr.ScheduleEventRelative(
                900 + actr.ActrRandom(200), "clear-exp-window",
                new dynamic[] {window.Title});

            AbstractAddCommandRequest addCommandRequest = new AddCommandRequest(list => KeyPressAction(list),
                "sperling-response",
                "KeyPressAction",
                "Sperling task key press response monitor");
            actr.AddDispatcherCommand(addCommandRequest);
            var modelDispatcherMonitor = new MonitorRequest("output-key", "sperling-response");
            actr.AddDispatcherMonitor(modelDispatcherMonitor);

            actr.Run(30, runInRealTime);

            actr.RemoveDispatcherMonitor("output-key", "sperling-response");
            actr.RemoveDispatcherCommand("KeyPressAction");

            var score = 0;

            foreach (var s in _responses)
                if (_answers.Contains(s.ToUpper()))
                    ++score;

            Console.Write("answers: [");
            foreach (var a in _answers) Console.Write("'" + a + "'");
            Console.Write("]\n");
            Console.Write("responses: [");
            foreach (var r in _responses) Console.Write("'" + r + "'");
            Console.Write("]\n");
            return score;
        }

        private static void KeyPressAction(dynamic[] list)
        {
            var pressedKey = (string) list[3];
            if (!pressedKey.Equals("space"))
                _responses.Add(pressedKey);
        }
    }
}