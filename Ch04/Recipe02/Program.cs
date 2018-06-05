using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe02
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskMethod("Main Thread Task");

            Task<int> task = CraeteTask("Task 1");
            task.Start();
            int result = task.Result;
            Console.WriteLine($"Result is : {result}");

            task = CraeteTask("Task 2");
            task.RunSynchronously();
            result = task.Result;
            Console.WriteLine($"Result is : {result}");

            task = CraeteTask("Task 3");
            task.Start();

            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            Console.WriteLine(task.Status);
            result = task.Result;
            Console.WriteLine($"Result is : {result}");
        }

        static Task<int> CraeteTask(string name)
        {
            return new Task<int>(() => TaskMethod(name));
        }

        private static int TaskMethod(string name)
        {
            Console.WriteLine($"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                              $"Is thread pool thread : {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 42;
        }
    }
}