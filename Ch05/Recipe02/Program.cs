using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe02
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
            Func<string, Task<string>> asyncLambda = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return $"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                       $" Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}";
            };

            string result = await asyncLambda("async lambda");

            Console.WriteLine(result);
        }
    }
}