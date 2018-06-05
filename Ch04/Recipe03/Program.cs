using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe03
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstTask = new Task<int>(() => TaskMethod("First task", 3));
            var secondTask = new Task<int>(() => TaskMethod("Second task", 2));

            firstTask.ContinueWith(t =>
            {
                Console.WriteLine(
                    $"The first answer is {t.Result}, Thread id {Thread.CurrentThread.ManagedThreadId}, " +
                    $" is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            
            firstTask.Start();
            secondTask.Start();
            
            Thread.Sleep(TimeSpan.FromSeconds(4));

            Task continuation = secondTask.ContinueWith(t =>
            {
                Console.WriteLine(
                    $"The second answer is {t.Result}, thread id {Thread.CurrentThread.ManagedThreadId}, " +
                    $" is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.ExecuteSynchronously);
            
            continuation.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine($"Continuation Task Completed ! Thread id {Thread.CurrentThread.ManagedThreadId}, " +
                                  $" is thread pool thread {Thread.CurrentThread.IsThreadPoolThread}");
            });
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.ReadLine();
            
            firstTask = new Task<int>(() =>
            {
                var innerTask = Task.Factory.StartNew(() => TaskMethod("Inner Second Task", 5),
                    TaskCreationOptions.AttachedToParent);

                innerTask.ContinueWith(t => TaskMethod("Inner Third Task", 2),
                    TaskContinuationOptions.AttachedToParent);

                return TaskMethod("Inner First Task", 2);
            });
            
            firstTask.Start();

            while (!firstTask.IsCompleted)
            {
                Console.WriteLine(firstTask.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            Console.WriteLine(firstTask.Status);
            
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine($"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                              $" Is thread pool : {Thread.CurrentThread.IsThreadPoolThread}");
            
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }
    }
}