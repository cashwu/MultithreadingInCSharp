using System;
using System.Diagnostics;
using System.Threading;

namespace Recipe03
{
    class Program
    {
        static void Main(string[] args)
        {
            const int numberOfOperation = 500;

            var sw = new Stopwatch();
            sw.Start();
            UserThreads(numberOfOperation);
            sw.Stop();
            Console.WriteLine($"Execution time using thread : {sw.ElapsedMilliseconds}");
            
            sw.Reset();
            sw.Start();
            UserThreadPool(numberOfOperation);
            sw.Stop();
            Console.WriteLine($"Execution time using thread : {sw.ElapsedMilliseconds}");
        }

        static void UserThreads(int numberOfOperations)
        {
            using (var countdown = new CountdownEvent(numberOfOperations))
            {
                Console.WriteLine($"Scheduling work by creating threads");
                for (int i = 0; i < numberOfOperations; i++)
                {
                    var thread = new Thread(() =>
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(TimeSpan.FromSeconds(0.1));
                        countdown.Signal();
                    });
                    thread.Start();
                }

                countdown.Wait();
                Console.WriteLine();
            }
        }

        static void UserThreadPool(int numberOfOperations)
        {
            using (var countdown = new CountdownEvent(numberOfOperations))
            {
                Console.WriteLine("Starting work on a threadpool");
                for (int i = 0; i < numberOfOperations; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(TimeSpan.FromSeconds(0.1));
                        countdown.Signal();
                    });
                }
                countdown.Wait();
                Console.WriteLine();
            }
        }
    }
}