using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe08
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronousProcessing();
            t.Wait();
        }

        static async Task AsynchronousProcessing()
        {
            var sync = new CustomAwaitable(true);
            string result = await sync;
            Console.WriteLine(result);
            
            var async = new CustomAwaitable(false);
            result = await async;
            Console.WriteLine(result);
        }
    }

    class CustomAwaitable
    {
        private readonly bool _completsSynchronously;

        public CustomAwaitable(bool completsSynchronously)
        {
            _completsSynchronously = completsSynchronously;
        }

        public CustomAwaiter GetAwaiter()
        {
            return new CustomAwaiter(_completsSynchronously);
        }
    }

    class CustomAwaiter : INotifyCompletion
    {
        private string _resut = "Completed synchronously";

        public CustomAwaiter(bool completsSynchronously)
        {
            IsCompleted = completsSynchronously;
        }

        public bool IsCompleted { get; }

        public void OnCompleted(Action continuation)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _resut = GetInfo();
                if (continuation != null)
                {
                    continuation();
                }
            });
        }

        private string GetInfo()
        {
            return $"Task is running on a thread is {Thread.CurrentThread.ManagedThreadId}, " +
                   $" Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}";
        }

        public string GetResult()
        {
            return _resut;
        }
    }
}