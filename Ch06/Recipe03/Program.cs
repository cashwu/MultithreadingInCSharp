using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe03
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = RunProgram();
            t.Wait();
        }

        static async Task RunProgram()
        {
            var taskStack = new ConcurrentStack<CustomTask>();
            var cts = new CancellationTokenSource();

            var taskSource = Task.Run(() => TaskProducr(taskStack));
            
            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProducr(taskStack, $"Processor {processorId}", cts.Token);
                });
            }

            await taskSource;
            
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            await Task.WhenAll(processors);
        }

        private static async Task TaskProducr(ConcurrentStack<CustomTask> stack, string name, CancellationToken token)
        {
            await GetRandomDelay();
            do
            {
                CustomTask workItem;
                bool popSuccesful = stack.TryPop(out workItem);
                if (popSuccesful)
                {
                    Console.WriteLine($"Task {workItem.Id} has been processed by {name}");
                }

                await GetRandomDelay();
            } while (!token.IsCancellationRequested);
        }

        private static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

        private static async Task TaskProducr(ConcurrentStack<CustomTask> stack)
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask {Id = i};
                stack.Push(workItem);

                Console.WriteLine($"Task {workItem.Id} has been posted");
            }
        }
    }

    internal class CustomTask
    {
        public int Id { get; set; }
    }
}