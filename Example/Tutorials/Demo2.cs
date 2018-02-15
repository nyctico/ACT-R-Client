using System;
using System.Collections.Generic;
using System.Threading;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.AddCommandRequests;
using Nyctico.Actr.Client.MonitorRequests;

namespace Nyctico.Actr.Example.Tutorials
{
    public static class Demo2
    {
        private static string _pressedKey = "";

        public static void Execute(bool human = false)
        {
            _pressedKey = "";
            var items = new[]
            {
                "B", "C", "D", "F", "G", "H", "J", "K", "L",
                "M", "N", "P", "Q", "R", "S", "T", "V", "W",
                "X", "Y", "Z"
            };
            using (var actr = new ActRClient("127.0.0.1", 2650))
            {
                actr.StartTraceMonitoring();
                actr.LoadActrModel("ACT-R:tutorial;unit2;demo2-model.lisp");
                actr.Reset();

                var numberList = new List<object>();
                for (var i = 0; i < items.Length; ++i) numberList.Add(i);
                var indexes = actr.PermuteList(numberList.ToArray());

                var targetItem = items[(long) indexes[0]];

                var window = actr.OpenExpWindow("Letter difference", true);

                actr.AddTextToWindow(window, targetItem, 125, 150);

                AbstractAddCommandRequest addCommandRequest = new AddCommandRequest(KeyPressAction,
                    "unit2-key-press",
                    "unit2-key-press",
                    "Assignment 2 task output-key monitor");

                actr.AddDispatcherCommand(addCommandRequest);
                var modelDispatcherMonitor = new MonitorRequest("output-key", "unit2-key-press");
                actr.AddDispatcherMonitor(modelDispatcherMonitor);

                if (!human)
                {
                    actr.InstallDevice(window);
                    actr.Run(10, true);
                }
                else
                {
                    while (_pressedKey.Equals("")) Thread.Sleep(1);
                }

                //actr.RemoveDispatcherMonitor("output-key", "unit2-key-press");
                actr.RemoveDispatcherMonitor(modelDispatcherMonitor);
                actr.RemoveDispatcherCommand("unit2-key-press");

                actr.StopTraceMonitoring();
                Console.WriteLine(targetItem.ToLower().Equals(_pressedKey.ToLower()));
            }
        }

        private static void KeyPressAction(object[] list)
        {
            _pressedKey = (string) list[3];
            Console.WriteLine($"Key pressed by model: {_pressedKey}");
        }
    }
}