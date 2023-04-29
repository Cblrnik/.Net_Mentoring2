using System;
using System.IO;

namespace DataCaptureService
{
    public class FolderWatcher
    {
        private readonly FileSystemWatcher _watcher;
        public event EventHandler<FileSystemEventArgs> NewSuitableFile = delegate { };

        public FolderWatcher(string pathToFolder)
        {
            _watcher = new FileSystemWatcher(pathToFolder ?? throw new ArgumentNullException(nameof(pathToFolder)))
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            _watcher.Created += OnCreated;
            _watcher.Renamed += OnCreated;
        }

        public void StartWatching()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            NewSuitableFile?.Invoke(sender, e);
        }
    }
}
