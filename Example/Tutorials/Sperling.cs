using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.DispatcherEvaluates;
using Nyctico.Actr.Client.DispatcherHooks;
using Nyctico.Actr.Client.DispatcherMonitors;

namespace Nyctico.Actr.Example.Tutorials
{
    public static class Sperling
    {
        private static List<string> _answers;
        private static List<string> _responses;
        private static readonly List<dynamic> Results = new List<dynamic> {0.0, 0.0, 0.0, 0.0};
        private static readonly List<dynamic> Onsets = new List<dynamic> {0.0, .15, .3, 1.0};

        public static void Execute(bool realTime, int numberOfRuns)
        {
            using (var actr = new ActRClient("127.0.0.1", 2650))
            {
                actr.StartTraceMonitoring(list => Console.WriteLine(
                    $"{(string) list[1]}: {((string) list[2]).Replace("\n", "")}"));
                actr.LoadModel("ACT-R:tutorial;unit3;sperling-model.lisp");
                actr.Reset();

                for (var i = 0; i < numberOfRuns; ++i) OneBlock(actr, realTime);

                var expData = new List<dynamic> {3.03, 2.4, 2.03, 1.5};
                for (var i = 0; i < Results.Count; ++i) Results[i] = Results[i] / numberOfRuns;

                actr.SendDispatcherEvaluate(new Correlation(Results, expData));
                actr.SendDispatcherEvaluate(new MeanDeviation(Results, expData));

                Console.WriteLine("Condition    Current Participant   Original Experiment");
                for (var i = 0; i < Results.Count; ++i)
                    Console.WriteLine(
                        $" {Onsets[i]:N2} sec.            {Results[i]:N2}                  {expData[i]:N2}");
            }
        }

        private static void OneBlock(ActRClient actr, bool runInRealTime)
        {
            var result = new List<double>();
            var permuteList = actr.SendDispatcherEvaluate(new PermuteList(Onsets)).ReturnValue[0];

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

            long row = actr.SendDispatcherEvaluate(new ActrRandom(3)).ReturnValue[0];

            var windowResult =
                actr.SendDispatcherEvaluate(new OpenExpWindow("Sperling Experiment", runInRealTime));
            var window = new Device(windowResult.ReturnValue[0].ToObject<List<dynamic>>());

            var letters = new List<dynamic>
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
            letters = ((JArray) actr.SendDispatcherEvaluate(new PermuteList(letters)).ReturnValue[0])
                .ToObject<List<dynamic>>();
            for (var i = 0; i < 3; ++i)
            for (var j = 0; j < 4; ++j)
            {
                var txt = letters[i + j * 4];
                if (i == row)
                    _answers.Add(txt);
                actr.SendDispatcherEvaluate(new AddTextToWindow(window, txt, 75 + j * 50, 101 + i * 50));
            }

            actr.SendDispatcherEvaluate(new InstallDevice(window));
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

            actr.SendDispatcherEvaluate(new NewToneSound(freq, 0.5, onset));
            actr.SendDispatcherEvaluate(new ScheduleSimpleEventRelative(
                900 + actr.SendDispatcherEvaluate(new ActrRandom(200)).ReturnValue[0], "clear-exp-window",
                new List<dynamic> {window.Infomation[2]}));

            AbstractDispatcherHook dispatcherHook = new LambdaDispatcherHook(list => KeyPressAction(list),
                "sperling-response",
                "KeyPressAction",
                "Sperling task key press response monitor");
            actr.AddDispatcherHook(dispatcherHook);

            var modelDispatcherMonitor = new DispatcherMonitor("output-key", "sperling-response");
            actr.AddDispatcherMonitor(modelDispatcherMonitor);

            actr.SendDispatcherEvaluate(new Run(30, runInRealTime));

            actr.RemoveDispatcherMonitor("output-keysperling-response");
            actr.RemoveDispatcherHook("KeyPressAction");

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

        private static void KeyPressAction(List<dynamic> list)
        {
            var pressedKey = (string) list[3];
            if (!pressedKey.Equals("space"))
                _responses.Add(pressedKey);
        }
    }
}