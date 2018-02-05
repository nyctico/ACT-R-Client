using System;
using System.Collections.Generic;
using Nyctico.Actr.Client;
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
                actr.StartTraceMonitoring();
                actr.LoadModel("ACT-R:tutorial;unit2;demo2-model.lisp");
                actr.Reset();

                var numberList = new List<dynamic>();
                for (var i = 0; i < items.Length; ++i) numberList.Add(i);
                var indexes = actr.PermuteList(numberList);

                var targetItem = items[indexes[0]];

                var window = actr.OpenExpWindow("Letter difference", true);

                actr.AddTextToWindow(window, targetItem, 125, 150);

                AbstractDispatcherHook dispatcherHook = new LambdaDispatcherHook(list => KeyPressAction(list),
                    "unit2-key-press",
                    "output-key",
                    "Assignment 2 task output-key monitor");

                actr.AddDispatcherHook(dispatcherHook);
                var modelDispatcherMonitor = new DispatcherMonitor("output-key", "unit2-key-press");
                actr.AddDispatcherMonitor(modelDispatcherMonitor);

                actr.InstallDevice(window);
                actr.Run(10, true);

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