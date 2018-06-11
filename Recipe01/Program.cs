using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe01
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.Invoke(
                () => EmulateProcessiong("Task1"),
                () => EmulateProcessiong("Task2"),
                () => EmulateProcessiong("Task3")
            );
            
            var cts = new CancellationTokenSource();

            var result = Parallel.ForEach(
                Enumerable.Range(1, 30),
                new ParallelOptions
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default
                },
                (i, state) =>
                {
                    Console.WriteLine(i);
                    if (i == 20)
                    {
                        state.Break();
                        Console.WriteLine($"Loop is stopped : {state.IsStopped}");
                    }
                });

            Console.WriteLine("---");
            Console.WriteLine($"IsCompleted : {result.IsCompleted}");
            Console.WriteLine($"Lowest break iteration : {result.LowestBreakIteration}");
        }

        static string EmulateProcessiong(string taskName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));

            Console.WriteLine($"{taskName} task was processed on a thread id {Thread.CurrentThread.ManagedThreadId}");

            return taskName;
        }
    }
}