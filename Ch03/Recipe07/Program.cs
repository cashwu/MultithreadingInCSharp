using System;
using System.ComponentModel;
using System.Threading;

namespace Recipe07
{
    class Program
    {
        static void Main(string[] args)
        {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += Worker_DoWork;
            bw.ProgressChanged += Worker_ProgressChanged;
            bw.RunWorkerCompleted += Worker_Completed;
            
            bw.RunWorkerAsync();

            Console.WriteLine("Press C to cancel work");

            do
            {
                if (Console.ReadKey(true).KeyChar == 'C')
                {
                    bw.CancelAsync();
                }
            } while (bw.IsBusy);
        }

        private static void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine($"Completed thread pool thrad id : {Thread.CurrentThread.ManagedThreadId}");

            if (e.Error != null)
            {
                Console.WriteLine($"Exception {e.Error.Message} has occured");
            }
            else if (e.Cancelled)
            {
                Console.WriteLine("Operation has been canceled");
            }
            else
            {
                Console.WriteLine($"The answer is {e.Result}");
            }
        }

        private static void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine($"{e.ProgressPercentage} % completed. Progress thread pool thread id : {Thread.CurrentThread.ManagedThreadId}");
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine($"DoWork thread pool thread id {Thread.CurrentThread.ManagedThreadId}");
            var bw = (BackgroundWorker) sender;
            for (int i = 0; i <= 100; i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (i % 10 == 0)
                {
                    bw.ReportProgress(i);
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            }

            e.Result = 42;
        }
    }
}