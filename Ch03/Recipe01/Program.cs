using System;
using System.Threading;

namespace Recipe01
{
    class Program
    {
        private delegate string RunOnThreadPool(out int threadId);
        
        static void Main(string[] args)
        {
            int threadId = 0;

            RunOnThreadPool poolDelegate = Test;
            
            var t  = new Thread(() => Test(out threadId));
            t.Start();
            t.Join();

            Console.WriteLine($"Thread id : {threadId}");

            IAsyncResult r = poolDelegate.BeginInvoke(out threadId, Callback, "a delegate asynchronous call");
            r.AsyncWaitHandle.WaitOne();

            string result = poolDelegate.EndInvoke(out threadId, r);

            Console.WriteLine($"Thread pool worker thread id : {threadId}");
            Console.WriteLine(result);
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("String a callback...");
            Console.WriteLine($"State passed to a callback {ar.AsyncState}");
            Console.WriteLine($"Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"Thread pool worker thread id : {Thread.CurrentThread.ManagedThreadId}");
        }

        private static string Test(out int threadId)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine($"Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            threadId = Thread.CurrentThread.ManagedThreadId;
            return $"Thread pool worker thread id was {threadId}";
        }
    }
}