using System;
using System.Threading;

namespace Recipe3
{
    class Program
    {
        static SemaphoreSlim _semaphore = new SemaphoreSlim(4);
        
        static void Main(string[] args)
        {
            for (int i = 0; i <= 6; i++)
            {
                string threadName = "Thread " + i;

                int secondsToWait = 2 + 2 * i;
                
                var t = new Thread(() => AccessDatabase(threadName, secondsToWait));
                t.Start();
            }
        }

        static void AccessDatabase(string name, int seconds)
        {
            Console.WriteLine($"{name} waits to access a database");

            _semaphore.Wait();
            
            Console.WriteLine($"{name} was granted an access to a database");
            
            Thread.Sleep(TimeSpan.FromSeconds(seconds));

            Console.WriteLine($"{name} is completed");

            _semaphore.Release();
        }
    }
}