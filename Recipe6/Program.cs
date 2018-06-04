using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Recipe6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Current thread priority : {Thread.CurrentThread.Priority}");
            Console.WriteLine("Running on all cores available");
            
            RunThreads();
            
            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.WriteLine("Running on a signle core");
            
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
            
            RunThreads();
        }

        static void RunThreads()
        {
            var sample = new ThreadSample();

            var threadOne = new Thread(sample.CountNumbers);
            threadOne.Name = "ThreadOne";
            
            var threadTwo = new Thread(sample.CountNumbers);
            threadTwo.Name = "ThreadTwo";

            threadOne.Priority = ThreadPriority.Highest;
            threadTwo.Priority = ThreadPriority.Lowest;
            
            threadOne.Start();
            threadTwo.Start();
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            sample.Stop();
        }
    }

    internal class ThreadSample
    {
        private bool _isStopped = false;

        public void CountNumbers()
        {
            long counter = 0;

            while (!_isStopped)
            {
                counter++;
            }

            Console.WriteLine($"{Thread.CurrentThread.Name} with {Thread.CurrentThread.Priority} has a count {counter:N0}");
        }

        public void Stop()
        {
            _isStopped = true;
        }
    }
}