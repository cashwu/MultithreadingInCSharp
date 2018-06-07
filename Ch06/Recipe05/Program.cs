using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Recipe05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using a Queue inside of BlockingCollection");
            Console.WriteLine();
            Task t = RunProgram();
            t.Wait();

            Console.WriteLine();
            Console.WriteLine("Using a Stack inside of BlockingCollection");
            Console.WriteLine();
            
            t = RunProgram(new ConcurrentStack<CustomTask>());
            t.Wait();
        }

        static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
        {
            var taskCollection = new BlockingCollection<CustomTask>();

            if (collection != null)
            {
                taskCollection = new BlockingCollection<CustomTask>(collection);
            }

            var taskSource = Task.Run(() => TaskProducer(taskCollection));
            
            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = $"Processor {i}";
                processors[i - 1] = Task.Run(() => TaskProducer(taskCollection, processorId));
            }

            await taskSource;

            await Task.WhenAll(processors);
        }

        private static async Task TaskProducer(BlockingCollection<CustomTask> collection)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(20);
                var workItem = new CustomTask { Id = i}; 
                collection.Add(workItem);
                Console.WriteLine($"Task {workItem.Id} have been posted");
            }
            
            collection.CompleteAdding();
        }

        private static async Task TaskProducer(BlockingCollection<CustomTask> collection, string name)
        {
            await GetRandomDelay();
            foreach (CustomTask item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine($"Task {item.Id} have been processed by {name}");
                await GetRandomDelay();
            }
        }

        private static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }
    }

    internal class CustomTask
    {
        public int Id { get; set; }
    }
}