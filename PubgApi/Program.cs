using System;

namespace PubgApi
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Controller main = new Controller();

            Console.WriteLine("Press ESC to stop...");
            Console.ReadLine();
            /*
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            */
        }
    }
}
