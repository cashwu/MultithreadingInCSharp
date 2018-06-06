using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe03
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronyWithTPL();
            t.Wait();

            t = AsynchronyWithAwait();
            t.Wait();
        }

        static Task AsynchronyWithTPL()
        {
            var containerTask = new Task(() =>
            {
                Task<string> t = GetInfoAsync("TPL 1");
                t.ContinueWith(task =>
                    {
                        Console.WriteLine(t.Result);

                        Task<string> t2 = GetInfoAsync("TPL 2");
                        t2.ContinueWith(innerTask => { Console.WriteLine(innerTask.Result); },
                            TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);

                        t2.ContinueWith(innterTask => { Console.WriteLine(innterTask.Exception.InnerExceptions); },
                            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
                    }, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);

                t.ContinueWith(task => { Console.WriteLine(t.Exception.InnerExceptions); },
                    TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
            });

            containerTask.Start();
            return containerTask;
        }

        static async Task AsynchronyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Async 1");
                Console.WriteLine(result);
                result = await GetInfoAsync("Async 2");
                Console.WriteLine(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            Console.WriteLine($"Task {name} started !");
            await Task.Delay(TimeSpan.FromSeconds(2));

            if (name == "TPL 2")
            {
                throw new Exception("Boom");
            }

            return $"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                   $" Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}