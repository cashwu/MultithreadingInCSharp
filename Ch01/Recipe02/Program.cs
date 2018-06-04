using System;
using System.Threading;

namespace Recipe2
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(PrintNumbersWithDelay);
            t.Start();
            PrintNumbers();
        }

        static void PrintNumbers()
        {
            Console.WriteLine("Starting");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }
        }
        
        static void PrintNumbersWithDelay()
        {
            Console.WriteLine("Starting");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine(i);
            }
        }
    }
}