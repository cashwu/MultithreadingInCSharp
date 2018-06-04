﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Recipe9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Incorrect counter");
            
            var c = new Counter();

            var t1 = new Thread(() => TestCount(c));
            var t2 = new Thread(() => TestCount(c));
            var t3 = new Thread(() => TestCount(c));
            
            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Console.WriteLine($"Total count: {c.Count}");
            Console.WriteLine("-----------------------");

            Console.WriteLine("Correct counter");
            
            var c1 = new CounterWithLock();
            
            t1 = new Thread(() => TestCount(c1));
            t2 = new Thread(() => TestCount(c1));
            t3 = new Thread(() => TestCount(c1));
            
            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            
            Console.WriteLine($"Total count: {c1.Count}");
        }

        static void TestCount(CounterBase c)
        {
            for (int i = 0; i < 100000; i++)
            {
                c.Increment();
                c.Decrement();
            } 
        }
    }

    class Counter : CounterBase
    {
        public int Count { get; private set; }
        public override void Increment()
        {
            Count++;
        }

        public override void Decrement()
        {
            Count--;
        }
    }
    
    class CounterWithLock : CounterBase
    {
        private readonly object _syncRoot = new object();
        public int Count { get; private set; }

        public override void Increment()
        {
            lock (_syncRoot)
            {
                Count++;
            }
        }

        public override void Decrement()
        {
            lock (_syncRoot)
            {
                Count--;
            }
        }
    }

    abstract class CounterBase
    {
        public abstract void Increment();
        public abstract void Decrement();
    }
}