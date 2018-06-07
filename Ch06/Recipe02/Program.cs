using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe02
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
            var taskQueue = new ConcurrentQueue<CustomTask>();
            var cts = new CancellationTokenSource();

            var taskSource = Task.Run(() => TaskProducer(taskQueue));
            
            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProducer(taskQueue, $"Processor {processorId}", cts.Token); 
                });
            }

            await taskSource;
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            await Task.WhenAll(processors);
        }

        private static async Task TaskProducer(ConcurrentQueue<CustomTask> queue, string name, CancellationToken token)
        {
            CustomTask workItem;
            bool deQueueSuccesful;

            await GetRandomDelay();

            do
            {
                deQueueSuccesful = queue.TryDequeue(out workItem);
                if (deQueueSuccesful)
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

        private static async Task TaskProducer(ConcurrentQueue<CustomTask> queue)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask() { Id = i }; 
                queue.Enqueue(workItem);

                Console.WriteLine($"Task {workItem.Id} has been posted");
            }
        }
    }

    internal class CustomTask
    {
        public int Id { get; set; }
    }
}