/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var queue = new ConcurrentQueue<int>();
            var isFinished = false;
            var semaphore = new Semaphore(1, 2);

            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    semaphore.WaitOne();
                    queue.Enqueue(i);
                    semaphore.Release();
                }

                isFinished = true;
            });
            Task.Factory.StartNew(() =>
            {
                while (!isFinished)
                {
                    semaphore.WaitOne();
                    Console.WriteLine($"[{string.Join(',', queue)}]");
                    semaphore.Release();
                }
            });

            Console.ReadLine();
        }
    }
}
