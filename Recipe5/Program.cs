using System;
using System.Threading;

namespace Recipe5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting program...");
            Thread t = new Thread(PrintNumbersWithStatus);
            Thread t2 = new Thread(DoNothing);
            Console.WriteLine(t.ThreadState.ToString());
            
            t2.Start();
            t.Start();

            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine(t.ThreadState);
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(6));
//            t.Abort();
            Console.WriteLine("A thread has been aborted");
            Console.WriteLine(t.ThreadState);
            Console.WriteLine(t2.ThreadState);
        }

        static void DoNothing()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        static void PrintNumbersWithStatus()
        {
            Console.WriteLine("Starting...");
            Console.WriteLine(Thread.CurrentThread.ThreadState);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine(i);
            }
        }
    }
}