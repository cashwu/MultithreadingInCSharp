using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Recipe03
{
    class Program
    {
        static void Main(string[] args)
        {
            var parallelQuery = from t in GetTypes().AsParallel()
                select EmulateProcessing(t);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(1));

            try
            {
                parallelQuery.WithDegreeOfParallelism(Environment.ProcessorCount)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .WithMergeOptions(ParallelMergeOptions.Default)
                    .WithCancellation(cts.Token)
                    .ForAll(Console.WriteLine);
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("----");
                Console.WriteLine("Operation has been canceled");
            }

            Console.WriteLine("----");
            Console.WriteLine("Unordered PLINQ query execution");

            var unorderQuery = from i in ParallelEnumerable.Range(1, 30)
                               select i;

            foreach (var i in unorderQuery)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("----");
            Console.WriteLine("Ordered PLINQ query exection");

            var orderedQuery = from i in ParallelEnumerable.Range(1, 30).AsOrdered()
                select i;
            
            foreach (var i in orderedQuery)
            {
                Console.WriteLine(i);
            }
        }

        static string EmulateProcessing(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            Console.WriteLine($"{typeName} type was processed on a thread id {Thread.CurrentThread.ManagedThreadId}");
            return typeName;
        }

        static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetExportedTypes()
                where type.Name.StartsWith("Web")
                orderby type.Name.Length
                select type.Name;
        }
    }
}