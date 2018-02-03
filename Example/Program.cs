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
                Console.WriteLine("(1) Demo2");
                Console.WriteLine("(q) Quit");
                Console.Write("> ");
                var option = Console.ReadLine();
                try
                {
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("Starting Demo2");
                            Console.WriteLine("------------------------------------------------");
                            Demo2.Execute();
                            Console.WriteLine("------------------------------------------------");
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
    }
}