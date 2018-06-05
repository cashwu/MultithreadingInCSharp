using System;
using System.ComponentModel;
using System.Threading;

namespace Recipe04
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperation1(token));
                Thread.Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }
            
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperation2(token));
                Thread.Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }
            
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperation3(token));
                Thread.Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        static void AsyncOperation1(CancellationToken token)
        {
            Console.WriteLine($"Starting the first task");
            for (int i = 0; i < 5; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"The first task has been canceled");
                    return;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine("The first task has completed succesfully");
        }

        static void AsyncOperation2(CancellationToken token)
        {
            try
            {
                Console.WriteLine($"Starting the second task");
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                Console.WriteLine("The second task has completed succesfully");
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("The second task has been canceled");
            }
        }

        static void AsyncOperation3(CancellationToken token)
        {
            bool cancellationFlag = false;

            token.Register(() => cancellationFlag = true);
            Console.WriteLine("Start the third task");

            for (int i = 0; i < 5; i++)
            {
                if (cancellationFlag)
                {
                    Console.WriteLine("The third task has been canceled"); 
                    return;
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine("The third task has completed succesfully");
        }
    }
}