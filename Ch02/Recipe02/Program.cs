using System;
using System.Threading;

namespace Recipe2
{
    class Program
    {
        private const string MutexName = "CSharpThreadingCookbook";
        
        static void Main(string[] args)
        {
            using (var m = new Mutex(false, MutexName))
            {
                if (!m.WaitOne(TimeSpan.FromSeconds(5), false))
                {
                    Console.WriteLine("Second instance is running"); 
                }
                else
                {
                    Console.WriteLine("Running");
                    Console.ReadLine();
                    m.ReleaseMutex();
                }
            }
        }
    }
}