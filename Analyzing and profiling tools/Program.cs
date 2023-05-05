using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Analyzing_and_profiling_tools
{
    public class Program
    {
        private const string Password = "P@SSWORD";
        static void Main(string[] args)
        {
            var salt = Encoding.UTF8.GetBytes("SoMeSaLtVaLuE1234567890");
            OldPerformance(salt);//410382
            NewPerformance(salt);//396220
        }

        public static void OldPerformance(byte[] salt)
        {
            var watcher = new Stopwatch();
            watcher.Start();
            FirstTask.GeneratePasswordHashUsingSaltOldImplementation(Password, salt);
            watcher.Stop();
            Console.WriteLine(watcher.ElapsedTicks);
        }

        public static void NewPerformance(byte[] salt)
        {
            var watcher = new Stopwatch();
            watcher.Start();
            FirstTask.GeneratePasswordHashUsingSaltNewImplementation(Password, salt);
            watcher.Stop();
            Console.WriteLine(watcher.ElapsedTicks);
        }

    }
}
