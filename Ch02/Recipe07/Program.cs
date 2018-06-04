using System;
using System.Threading;

namespace Recipe07
{
    class Program
    {
        static Barrier _barrier = new Barrier(2,
            barrier => { Console.WriteLine($"End of phase {barrier.CurrentPhaseNumber + 1}"); });

        static void Main(string[] args)
        {
            var t1 = new Thread(() => PlayMusic("the guitarist", "play an amazing solo", 5));
            var t2 = new Thread(() => PlayMusic("the singer", "sing his song", 2));
            
            t1.Start();
            t2.Start();
        }

        static void PlayMusic(string name, string msg, int seconds)
        {
            for (int i = 1; i < 3; i++)
            {
                Console.WriteLine("---------------------"); 
                
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                Console.WriteLine($"{name} starts to {msg}");
                
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                Console.WriteLine($"{name} finishes to {msg}");

                _barrier.SignalAndWait();
            }
        }
    }
}