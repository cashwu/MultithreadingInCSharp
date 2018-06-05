using System;
using System.Threading;

namespace Recipe05
{
    class Program
    {
        static void Main(string[] args)
        {
            RunOperations(TimeSpan.FromSeconds(5));
            RunOperations(TimeSpan.FromSeconds(7));
        }

        static void RunOperations(TimeSpan workerOperationTimeout)
        {
            using (var evt = new ManualResetEvent(false))
            using (var cts = new CancellationTokenSource())
            {
                Console.WriteLine("Registering timeout operations..");
                var worker = ThreadPool.RegisterWaitForSingleObject(evt, (state, isTimeout) =>
                    {
                        WorkerOperationWait(cts, isTimeout);
                    }, null, workerOperationTimeout, true);

                Console.WriteLine("Starting long running operation..");
                ThreadPool.QueueUserWorkItem(_ => WorkerOperation(cts.Token, evt));
                
                Thread.Sleep(workerOperationTimeout.Add(TimeSpan.FromSeconds(2)));
                worker.Unregister(evt);
            }
        }

        private static void WorkerOperation(CancellationToken token, ManualResetEvent evt)
        {
            for (int i = 0; i < 6; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            evt.Set();
        }

        private static void WorkerOperationWait(CancellationTokenSource cts, bool isTimeout)
        {
            if (isTimeout)
            {
                cts.Cancel();
                Console.WriteLine("Worker operation timed out and was canceled");
            }
            else
            {
                Console.WriteLine("Worker operation succeded");
            }
        }
    }
}