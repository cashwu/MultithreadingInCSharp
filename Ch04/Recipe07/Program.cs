using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe07
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> task;
            try
            {
                task = Task.Run(() => TaskMethod("Task 1", 2));
                int result = task.Result;
                Console.WriteLine($"Result : {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught : {ex}");
            }

            Console.WriteLine("--------------");
            Console.WriteLine();

            try
            {
                task = Task.Run(() => TaskMethod("Task 2", 2));
                int result = task.GetAwaiter().GetResult();
                Console.WriteLine($"Result ; {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught : {ex}");
            }
            
            Console.WriteLine("--------------");
            Console.WriteLine();

            var t1 = new Task<int>(() => TaskMethod("Task 3", 3));
            var t2 = new Task<int>(() => TaskMethod("Task 4", 2));

            var complexTask = Task.WhenAll(t1, t2);
            var exceptionHandler = complexTask.ContinueWith(
                t =>
                {
                    Console.WriteLine($"Exception caught : {t.Exception}"); 
                },
                TaskContinuationOptions.OnlyOnFaulted);
            
            t1.Start();
            t2.Start();
            
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine($"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                              $" Is thread pool thread {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception("Boom");
            return 42 * seconds;
        }
    }
}