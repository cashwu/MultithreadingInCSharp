using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Recipe01
{
    class Program
    {
        private const string Item = "Dictionary item";
        public static string CurrentItem;
        
        static void Main(string[] args)
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();

            var sw = new Stopwatch();
            
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                } 
            }
            sw.Stop();

            Console.WriteLine($"Writing to dictionary with a lock : {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                concurrentDictionary[i] = Item;
            }
            sw.Stop();

            Console.WriteLine($"Writing to a concurrent dictionary : {sw.Elapsed}");
            
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }
            sw.Stop();

            Console.WriteLine($"Reading from dictionary with a lock : {sw.Elapsed}");
            
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }
            sw.Stop();

            Console.WriteLine($"Reading from a concurrent dictionary : {sw.Elapsed}");
        }
    }
}