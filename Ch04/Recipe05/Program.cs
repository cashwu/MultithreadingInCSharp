using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe05
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcs = new TaskCompletionSource<int>();

            var worker = new BackgroundWorker();

            worker.DoWork += (sender, eventArgs) =>
            {
                eventArgs.Result = TaskMethod("Background worker", 5); 
            };
            
            worker.RunWorkerCompleted += (sender, eventArgs) =>
            {
                if (eventArgs.Error != null)
                {
                    tcs.SetException(eventArgs.Error);
                }
                else if (eventArgs.Cancelled)
                {
                    tcs.SetCanceled();
                }
                else
                {
                    tcs.SetResult((int)eventArgs.Result);
                }
            };

            worker.RunWorkerAsync();

            int result = tcs.Task.Result;

            Console.WriteLine($"Result is : {result}");
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine($"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                              $" Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }
    }
}