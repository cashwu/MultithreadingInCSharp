using System;
using System.Threading;

namespace Recipe5
{
    class Program
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);
        
        static void Main(string[] args)
        {
            var t1 = new Thread(() => TravelThroughGates("Thread 1", 5));
            var t2 = new Thread(() => TravelThroughGates("Thread 2", 6));
            var t3 = new Thread(() => TravelThroughGates("Thread 3", 12));
            
            t1.Start();
            t2.Start();
            t3.Start();

            Thread.Sleep(TimeSpan.FromSeconds(6));

            Console.WriteLine("The gates are now open");
            
            _mainEvent.Set();
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            
            _mainEvent.Reset();

            Console.WriteLine("The gates have been closed");
            
            Thread.Sleep(TimeSpan.FromSeconds(10));

            Console.WriteLine($"The gates are now open for the second time");
            
            _mainEvent.Set();
            
            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.WriteLine("The gates have been closed");
            
            _mainEvent.Reset();
        }

        static void TravelThroughGates(string name, int seconds)
        {
            Console.WriteLine($"{name} falls to sleep");
            
            Thread.Sleep(TimeSpan.FromSeconds(seconds));

            Console.WriteLine($"{name} waits for the gates to open");

            _mainEvent.Wait();

            Console.WriteLine($"{name} enters the gates");
        }
    }
}