using System;
using System.IO;
using System.Net;
using Confluent.Kafka;

namespace DataCaptureService
{
    public class Producer
    {
        private const string BootstrapServers = "localhost:9092";
        private const string Topic = "test";

        public Producer(FolderWatcher watcher)
        {
            watcher.NewSuitableFile += SendFile;
        }

        private void SendFile(object sender, FileSystemEventArgs eventArgs)
        {
            var file = File.ReadAllBytes(eventArgs.FullPath);

            SendOrderRequest(eventArgs.Name, file);
        }

        public void SendOrderRequest(string fileName, byte[] message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = BootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using var producer = new ProducerBuilder<string, byte[]>(config).Build();

                var messages = new Message<string, byte[]>
                {
                    Key = fileName,
                    Value = message
                };


                var result = producer.ProduceAsync(Topic, messages).GetAwaiter().GetResult();
                if (result != null)
                {
                    Console.WriteLine($"File {result.Key} was sent.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }
}
