using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe07
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncTask();
            t.Wait();
            
            AsyncVoid();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            t = AsyncTaskWithErrors();
            while (!t.IsFaulted)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine(t.Exception);

//            try
//            {
//                AsyncVoidWithErrors();
//                Thread.Sleep(TimeSpan.FromSeconds(3));
//            }
//            catch (Exception exception)
//            {
//                Console.WriteLine(exception);
//            }

//            int[] numbers = {1, 2, 3, 4, 5};
//            Array.ForEach(numbers, async number =>
//            {
//                await Task.Delay(TimeSpan.FromSeconds(1));
//                if (number == 3)
//                {
//                    throw new Exception("Boom");
//                }
//
//                Console.WriteLine(number);
//            });

            Console.ReadLine();
        }

        static async Task AsyncTaskWithErrors()
        {
            string result = await GetInfoAsync("AsyncTaskException", 2);
            Console.WriteLine(result);
        }

        static async void AsyncVoidWithErrors()
        {
            string result = await GetInfoAsync("AsyncVoidException", 2);
            Console.WriteLine(result);
        }

        static async Task AsyncTask()
        {
            string result = await GetInfoAsync("AsyncTask", 2);
            Console.WriteLine(result);
        }

        static async void AsyncVoid()
        {
            string result = await GetInfoAsync("AsyncVoid", 2);
            Console.WriteLine(result);
        }

        private static async Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            if (name.Contains("Exception"))
            {
                throw new Exception($"Boom form {name}");
            }

            return $"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId} " +
                   $" Is thread pool thread {Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}