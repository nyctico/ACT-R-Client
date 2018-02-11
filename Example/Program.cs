using System;
using Nyctico.Actr.Example.Tutorials;

namespace Nyctico.Actr.Example
{
    internal static class Program
    {
        public static void Main()
        {
            var quitNow = false;
            while (!quitNow)
            {
                Console.WriteLine("Which task should be performed?");
                Console.WriteLine("(2) Demo2");
                Console.WriteLine("(3) Sperling");
                Console.WriteLine("(q) Quit");
                Console.Write("> ");
                var option = Console.ReadLine();
                try
                {
                    switch (option)
                    {
                        case "2":
                            StartDemo2();
                            break;

                        case "3":
                            StartSperling();
                            break;

                        case "q":
                            quitNow = true;
                            break;

                        default:
                            Console.WriteLine($"Unknown Command {option}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void StartDemo2()
        {
            Console.WriteLine("(1) Human");
            Console.WriteLine("(2) Model");
            Console.WriteLine("(q) Quit");
            Console.Write("> ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine($"Starting Demo2 with human participant");
                    Console.WriteLine("------------------------------------------------");
                    Demo2.Execute(true);
                    Console.WriteLine("------------------------------------------------");
                    break;

                case "2":
                    Console.WriteLine($"Starting Demo2 with model participant");
                    Console.WriteLine("------------------------------------------------");
                    Demo2.Execute();
                    Console.WriteLine("------------------------------------------------");
                    break;

                case "q":
                    return;

                default:
                    Console.WriteLine($"Unknown Command {option}");
                    break;
            }
        }

        private static void StartSperling()
        {
            Console.WriteLine("(1) Real time");
            Console.WriteLine("(2) Fast forward");
            Console.WriteLine("(q) Quit");
            Console.Write("> ");
            var option = Console.ReadLine();
            int numberOfBlocks;
            switch (option)
            {
                case "1":
                    numberOfBlocks = NumberOfBlocks();
                    Console.WriteLine($"Starting Sperling with {numberOfBlocks} Blocks in Real Time");
                    Console.WriteLine("------------------------------------------------");
                    Sperling.Execute(true, numberOfBlocks);
                    Console.WriteLine("------------------------------------------------");
                    break;

                case "2":
                    numberOfBlocks = NumberOfBlocks();
                    Console.WriteLine($"Starting Sperling with {numberOfBlocks} Blocks");
                    Console.WriteLine("------------------------------------------------");
                    Sperling.Execute(false, numberOfBlocks);
                    Console.WriteLine("------------------------------------------------");
                    break;

                case "q":
                    return;

                default:
                    Console.WriteLine($"Unknown Command {option}");
                    break;
            }
        }

        private static int NumberOfBlocks()
        {
            Console.WriteLine("Please enter number of blocks:");
            Console.Write("> ");
            int numberOfBlocks;
            while (!int.TryParse(Console.ReadLine(), out numberOfBlocks))
                Console.WriteLine("Please enter number of blocks:");
            return numberOfBlocks;
        }
    }
}