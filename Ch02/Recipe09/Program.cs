﻿using System;
using System.Threading;

namespace Recipe09
{
    class Program
    {
        private static volatile bool _isCompleted = false;
        
        static void Main(string[] args)
        {
            var t1 = new Thread(UserModeWait);
            var t2 = new Thread(HybridSpinWait);

            Console.WriteLine("Running user mode waiting");
            
            t1.Start();
            
            Thread.Sleep(20);

            _isCompleted = true;
            
            Thread.Sleep(TimeSpan.FromSeconds(1));

            _isCompleted = false;

            Console.WriteLine("Running hybrid SpinWait construct waiting");
            
            t2.Start();
            
            Thread.Sleep(5);

            _isCompleted = true;
        }

        static void UserModeWait()
        {
            while (!_isCompleted)
            {
                Console.WriteLine(".");
            }

            Console.WriteLine();
            Console.WriteLine("Waiting is complete");
        }

        static void HybridSpinWait()
        {
            var w = new SpinWait();

            while (!_isCompleted)
            {
                w.SpinOnce();
                Console.WriteLine(w.NextSpinWillYield);
            }

            Console.WriteLine("Waiting is complete");
        }
    }
}