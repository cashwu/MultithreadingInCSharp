using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe08
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstTask = new Task<int>(() => TaskMethod("First Task", 2));
            var secondTask = new Task<int>(() => TaskMethod("Second Task", 2));
            var whenAllTask = Task.WhenAll(firstTask, secondTask);

            whenAllTask.ContinueWith(t =>
            {
                Console.WriteLine($"The first answer is {t.Result[0]}, the second is {t.Result[1]}");
            });

            firstTask.Start();
            secondTask.Start();
            
            Thread.Sleep(TimeSpan.FromSeconds(4));

            var tasks = new List<Task<int>>();
            for (int i = 0; i < 4; i++)
            {
                int counter = i;
                var task = new Task<int>(() => TaskMethod($"Task {counter}", counter));
                 
                tasks.Add(task);
                task.Start();
            }

            while (tasks.Count > 0)
            {
                var completedTask = Task.WhenAny(tasks).Result;
                tasks.Remove(completedTask);

                Console.WriteLine($"A task has been completed with result {completedTask.Result}");
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine($"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}, " +
                              $" Is thread pool thread {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }
    }
}