using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ImpromptuInterface;

namespace Recipe09
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
            string result = await GetDynamicAwaitableObject(true);
            Console.WriteLine(result);

            result = await GetDynamicAwaitableObject(false);
            Console.WriteLine(result);
        }

        static dynamic GetDynamicAwaitableObject(bool completsSynchronously)
        {
            dynamic result = new ExpandoObject();
            dynamic awaiter = new ExpandoObject();

            awaiter.Message = "Completed synchronously";
            awaiter.IsCompleted = completsSynchronously;
            awaiter.GetResult = (Func<string>) (() => awaiter.Message);

            awaiter.OnCompleted = (Action<Action>) (callback =>
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    awaiter.Message = GetInfo();
                    callback?.Invoke();
                });
            });

            IAwaiter<string> proxy = Impromptu.ActLike(awaiter);

            result.GetAwaiter = (Func<dynamic>) (() => proxy);

            return result;
        }

        static object GetInfo()
        {
            return $"Task is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                   $" Is thread pool thread {Thread.CurrentThread.IsThreadPoolThread}";
        }
    }

    public interface IAwaiter<T> : INotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
    }
}