using System;
using System.Threading;

namespace Recipe02
{
    class Program
    {
        static void Main(string[] args)
        {
            const int x = 1;
            const int y = 2;
            const string lambdaState = "lambda state 2";

            ThreadPool.QueueUserWorkItem(AsyncOperation);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            ThreadPool.QueueUserWorkItem(AsyncOperation, "async statr");
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"operation state : {state}");
                Console.WriteLine($"worker thread id : {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine($"operation state : {x + y}, {lambdaState}");
                Console.WriteLine($"worker thread id : {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private static void AsyncOperation(object state)
        {
            Console.WriteLine($"Operation state : {state ?? "(null)"}");
            Console.WriteLine($"Worker thread id : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}