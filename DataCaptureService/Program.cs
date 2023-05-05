using System;

namespace DataCaptureService
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var watcher = new FolderWatcher("D:\\NetMentoring\\DataCaptureService\\bin\\Debug\\Test");
            var producer = new Producer(watcher);
            watcher.StartWatching();

            while (true)
            {
                var keypress = Console.ReadKey();

                if ((ConsoleModifiers.Control & keypress.Modifiers) == 0) continue;
                if (keypress.Key != ConsoleKey.Q) continue;
                watcher.StopWatching();
                Environment.Exit(0);
            }
        }
    }
}
