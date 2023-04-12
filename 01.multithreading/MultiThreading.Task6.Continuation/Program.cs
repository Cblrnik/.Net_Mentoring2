/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            FirstTask();
            Thread.Sleep(100);
            Console.WriteLine();
            SecondTask();
            Thread.Sleep(100);
            Console.WriteLine();
            ThirdTask();
            Thread.Sleep(100);
            Console.WriteLine();
            ForthTask();

            Console.ReadLine();
        }

        public static void FirstTask()
        {
            Console.WriteLine("Start first task");
            var source = new TaskCompletionSource<object>();
            var continuationTask = source.Task;
            source.SetException(new Exception("Some test exception"));

            continuationTask.ContinueWith(task1 => OutputForTask("regardless of the result of the parent task"));
        }

        public static void SecondTask()
        {
            Console.WriteLine("Start second task");
            var source = new TaskCompletionSource<object>();
            var continuationTask = source.Task;
            source.SetCanceled();

            continuationTask.ContinueWith(task => OutputForTask("when the parent task finished without success"));

            var sourceForException = new TaskCompletionSource<object>();
            var continuationForException = sourceForException.Task;
            sourceForException.SetException(new Exception("Some test exception"));

            continuationForException.ContinueWith(task => OutputForTask("when the parent task finished without success"));
        }

        public static void ThirdTask()
        {
            Console.WriteLine("Start third task");
            var source = new TaskCompletionSource<object>();
            var continuationTask = source.Task;
            source.SetException(new Exception("Some test exception"));

            continuationTask.ContinueWith((task) =>
            {
                OutputForTask(
                    "when the parent task would be finished with fail and parent task thread should be reused for continuation");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        public static void ForthTask()
        {
            Console.WriteLine("Start forth task");
            var source = new TaskCompletionSource<object>();
            var continuationTask = source.Task;
            source.SetCanceled();


            continuationTask.ContinueWith((task) =>
            {
                var thread = new Thread(() => OutputForTask("outside of the thread pool when the parent task would be cancelled"));
                thread.Start();

            }, TaskContinuationOptions.OnlyOnCanceled);
        }

        private static void OutputForTask(string text)
        {
            Console.WriteLine($"Works {text}");
        }
    }
}
