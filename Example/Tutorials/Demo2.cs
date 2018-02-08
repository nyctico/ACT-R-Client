using System;
using System.Collections.Generic;
using System.Threading;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.HookRequests;
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

                var numberList = new List<dynamic>();
                for (var i = 0; i < items.Length; ++i) numberList.Add(i);
                var indexes = actr.PermuteList(numberList);

                var targetItem = items[indexes[0]];

                var window = actr.OpenExpWindow("Letter difference", true);

                actr.AddTextToWindow(window, targetItem, 125, 150);
                
                var line1 = actr.AddLineToExpWindow(window,new int[]{10,10},new int[]{
                    20,20
                });
                
                var line2 = actr.AddLineToExpWindow(window,new int[]{30,30},new int[]{
                    40,40
                }, "green");
                
                var line3 = actr.AddLineToExpWindow(window,new int[]{50,50},new int[]{
                    60,60
                }, "red");

                actr.ModifyLineForExpWindow(line3,new int[]{50,50},new int[]{100,100},"blue");
                
                AbstractHookRequest hookRequest = new LambdaHookRequest(list => KeyPressAction(list),
                    "unit2-key-press",
                    "output-key",
                    "Assignment 2 task output-key monitor");

                actr.AddDispatcherHook(hookRequest);
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