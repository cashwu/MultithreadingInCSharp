using System;
using System.Threading;

namespace Recipe06
{
    class Program
    {
        private static Timer _timer;
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'Enter' to stop the timer..");
            
            DateTime start = DateTime.Now;
            
            _timer = new Timer(_ => TimerOperation(start), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)); 
            
            Thread.Sleep(TimeSpan.FromSeconds(6));

            _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(4));

            Console.ReadLine();

            _timer.Dispose();
        }

        static void TimerOperation(DateTime start)
        {
            TimeSpan elapsed = DateTime.Now - start;
            Console.WriteLine($"{elapsed.Seconds} seconds from {start} Timer thread pool thread id {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}