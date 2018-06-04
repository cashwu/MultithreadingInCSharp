using System;
using System.Threading;

namespace Recipe3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            Thread t = new Thread(PrintNumbersWithDelay);
            t.Start();
            t.Join();
            Console.WriteLine("Thread Completed");
        }

        static void PrintNumbersWithDelay()
        {
            Console.WriteLine("Starting...");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine(i);
            }
        }
    }
}