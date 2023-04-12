/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static readonly int RecursiveNumber = 10;
        private static readonly Semaphore Semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();
            FirstTask();
            SecondTask(RecursiveNumber);
            // feel free to add your code

            Console.ReadLine();
        }

        public static void FirstTask()
        {
            var thread = CreateThread();
            thread.Start(RecursiveNumber);
            thread.Join();
        }

        private static void SecondTask(int recursiveNumber)
        {
            ThreadPool.QueueUserWorkItem((number) =>
            {
                Semaphore.WaitOne();
                var castedNumber = (int)number;
                if (castedNumber <= 0)
                {
                    return;
                }

                castedNumber = DecrementNumber(castedNumber);
                SecondTask(castedNumber);
                Semaphore.Release();
            }, recursiveNumber);
        }

        private static Thread CreateThread()
        {
            return new Thread((number) =>
            {
                var castedNumber = (int)number;
                if (castedNumber <= 0)
                {
                    return;
                }

                castedNumber = DecrementNumber(castedNumber);

                var newThread = CreateThread();
                newThread.Start(castedNumber);
                newThread.Join();
            });
        }

        private static int DecrementNumber(int number)
        {
            Console.WriteLine(--number);
            return number;
        }
    }
}
