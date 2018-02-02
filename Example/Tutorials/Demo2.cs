using System;
using System.Collections.Generic;
using System.Linq;
using Nyctico.Actr.Client;
using Nyctico.Actr.Client.Abstracts;
using Nyctico.Actr.Client.Commands;
using Nyctico.Actr.Client.Data;
using Nyctico.Actr.Client.DispatcherCommands;

namespace Nyctico.Actr.Example.Tutorials
{
    public static class Demo2
    {
        public static void Execute()
        {
            using (ActRClient actr = new ActRClient("127.0.0.1", 2650))
            {
                actr.StartTraceMonitoring(list => Console.WriteLine(
                    $"{(string) list[1]}: {((string) list[2]).Replace("\n", "")}"));
                actr.LoadModel("ACT-R:tutorial;unit2;demo2-model.lisp");
                actr.Reset();
                
                string[] items = {"B","C","D","F","G","H","J","K","L",
                    "M","N","P","Q","R","S","T","V","W",
                    "X","Y","Z"};

                Result indexes = actr.SendDispatcherCommand(new PermuteList
                {
                    Indexes = Enumerable.Range(0, items.Length - 1).ToList()
                });

                string targetItem = items[indexes.ReturnValue[0][0]];

                Result window = actr.SendDispatcherCommand(new OpenExpWindow
                {
                    Title = "Letter difference",
                    Visible = true,
                    Width = 300,
                    Height = 300,
                    X = 300,
                    Y = 300
                });
                
                actr.SendDispatcherCommand(new AddTextToWindow
                {
                    Window = window.ReturnValue[0].ToObject<List<dynamic>>(),
                    Text = targetItem,
                    X = 125,
                    Y = 150,
                    Color = "black",
                    Height = 20,
                    Width = 75,
                    FontSize = 12
                });
                
                string lastPressedKey = "";
                AbstractCommand command = new LambdaCommand("unit2-key-press", "output-key", "Assignment 2 task output-key monitor")
                {
                    SingelInstance = "true",
                    ExecFunc = list =>
                    {
                        Console.WriteLine($"Key pressed by model: {(string) list[3]}");
                        lastPressedKey = (string) list[3];
                    },
                    LispCmd = null
                };
                
                actr.Add(command);
                
                MonitorCommand modelMonitor = new MonitorCommand("output-key", "unit2-key-press");
                actr.Add(modelMonitor);

                actr.SendDispatcherCommand(new InstallDevice
                {
                    Window = new List<object>{window.ReturnValue[0][0].ToString(),window.ReturnValue[0][1].ToString(),window.ReturnValue[0][2].ToString()},
                });
                
                actr.SendDispatcherCommand(new Run
                {
                    Time = 10,
                    RealTime = true
                });
                
                actr.RemoveMonitor("output-keyunit2-key-press");
                actr.RemoveCommand("output-key");
                
                actr.StopTraceMonitoring();
                Console.WriteLine(targetItem.ToLower().Equals(lastPressedKey.ToLower()));
            }
        }
    }
}