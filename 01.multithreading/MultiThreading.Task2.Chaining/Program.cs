/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var first = Task.Factory.StartNew(() =>
            {
                var array = new int[10];
                var random = new Random();
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = random.Next(1,100);
                    Console.WriteLine(array[i]);
                }

                Console.WriteLine();
                return array;
            });

            var second = Task.Factory.StartNew(() =>
            {
                var random = new Random();
                var randomNumber = random.Next(1, 100);
                var arrToMult = first.Result;
                for (var i = 0; i < arrToMult.Length; i++)
                {
                    arrToMult[i] *= randomNumber;
                    Console.WriteLine(arrToMult[i]);
                }
                Console.WriteLine();
                return arrToMult;

            });

            var third = Task.Factory.StartNew(() =>
            {
                var arrToSort = second.Result;

                return arrToSort.OrderBy(x => x).Select(x =>
                {
                    Console.WriteLine(x);
                    return x;
                }).ToArray();

            });

            var forth = Task.Factory.StartNew(() =>
            {

                var result = third.Result;
                var average = result.Average();
                Console.WriteLine("Average: " + average);
            });

            Console.ReadLine();
        }
    }
}
