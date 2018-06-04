﻿using System;
using System.Threading;

namespace Recipe11
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Thread(FaultyThread);
            
            t.Start();
            t.Join();

            try
            {
                t = new Thread(BadFaultyThread);
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("We won't get here");
            }
        }

        static void BadFaultyThread()
        {
            Console.WriteLine("Starting a bad faulty thread..");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            throw new Exception("Boom");
        }

        static void FaultyThread()
        {
            try
            {
                Console.WriteLine("Starting a faulty thread..");
                Thread.Sleep(TimeSpan.FromSeconds(1));
                throw new Exception("Boom");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception handled: {e.Message}");
            }
        }
    }
}