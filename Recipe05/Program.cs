using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace Recipe05
{
    class Program
    {
        static void Main(string[] args)
        {
            var partitioner = new StringPartitioner(GetTypes());

            var parallelQuery = from t in partitioner.AsParallel()
                select EmulateProcessing(t);

            parallelQuery.ForAll(PrintInfo);
        }

        static void PrintInfo(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine($"{typeName} type was printed on a thread id {Thread.CurrentThread.ManagedThreadId}");
        }

        static string EmulateProcessing(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine($"{typeName} type was processed on a thred id {Thread.CurrentThread.ManagedThreadId} " +
                              $" Has {(typeName.Length % 2 == 0 ? "even" : "odd")} length");

            return typeName;
        }

        static IEnumerable<string> GetTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetExportedTypes());

            return from type in types
                where type.Name.StartsWith("Web")
                select type.Name;
        }
    }

    public class StringPartitioner : Partitioner<string>
    {
        private readonly IEnumerable<string> _data;

        public StringPartitioner(IEnumerable<string> data)
        {
            foreach (var d in data)
            {
                Console.WriteLine(d);
            }
            _data = data;
        }

        public override bool SupportsDynamicPartitions => false;

        public override IList<IEnumerator<string>> GetPartitions(int partitionCount)
        {
            var result = new List<IEnumerator<string>>(2);
            result.Add(CreateEnumerator(true));
            result.Add(CreateEnumerator(false));

            return result;
        }

        IEnumerator<string> CreateEnumerator(bool isEven)
        {
            foreach (var d in _data)
            {
                if (!(d.Length % 2 == 0 ^ isEven))
                yield return d;
            }
        }
    }
}