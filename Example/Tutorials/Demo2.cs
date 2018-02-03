using System;
using System.Collections.Generic;
using System.Linq;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.DispatcherEvaluates;
using Nyctico.Actr.Client.DispatcherHooks;
using Nyctico.Actr.Client.DispatcherMonitors;

namespace Nyctico.Actr.Example.Tutorials
{
    public static class Demo2
    {
        private static string _pressedKey = "";

        public static void Execute()
        {
            var items = new[]
            {
                "B", "C", "D", "F", "G", "H", "J", "K", "L",
                "M", "N", "P", "Q", "R", "S", "T", "V", "W",
                "X", "Y", "Z"
            };
            using (var actr = new ActRClient("127.0.0.1", 2650))
            {
                actr.StartTraceMonitoring(list => Console.WriteLine(
                    $"{(string) list[1]}: {((string) list[2]).Replace("\n", "")}"));
                actr.LoadModel("ACT-R:tutorial;unit2;demo2-model.lisp");
                actr.Reset();

                var indexes =
                    actr.SendDispatcherEvaluate(new PermuteList(Enumerable.Range(0, items.Length - 1).ToList()));

                var targetItem = items[indexes.ReturnValue[0][0]];

                var windowResult =
                    actr.SendDispatcherEvaluate(new OpenExpWindow("Letter difference", true, 300, 300, 300, 300));
                var window = new Device(windowResult.ReturnValue[0].ToObject<List<dynamic>>());

                actr.SendDispatcherEvaluate(new AddTextToWindow(window, targetItem, 125, 150, "black", 50, 75, 12));

                AbstractDispatcherHook dispatcherHook = new LambdaDispatcherHook(list => KeyPressAction(list),
                    "unit2-key-press",
                    "output-key",
                    "Assignment 2 task output-key monitor");

                actr.AddDispatcherHook(dispatcherHook);

                var modelDispatcherMonitor = new DispatcherMonitor("output-key", "unit2-key-press");
                actr.AddDispatcherMonitor(modelDispatcherMonitor);

                actr.SendDispatcherEvaluate(new InstallDevice(window));

                actr.SendDispatcherEvaluate(new Run(10, true));

                actr.RemoveDispatcherMonitor("output-keyunit2-key-press");
                actr.RemoveDispatcherHook("output-key");

                actr.StopTraceMonitoring();
                Console.WriteLine(targetItem.ToLower().Equals(_pressedKey.ToLower()));
            }
        }

        private static void KeyPressAction(List<dynamic> list)
        {
            _pressedKey = (string) list[3];
            Console.WriteLine($"Key pressed by model: {_pressedKey}");
        }
    }
}