using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Confluent.Kafka;

namespace DataCaptureService
{
    public class Producer
    {
        private const string BootstrapServers = "localhost:9092";
        private const string Topic = "test";
        private const int Size = 100_048;
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

                if (message.Length >= Size)
                {
                    var indexer = 0;
                    var contents = GetContentSequence();
                    foreach (var contentToSend in contents)
                    {
                        SendMessage(producer, contentToSend, ++indexer, contents.Count);
                    }
                }
                else
                {
                    SendMessage(producer, message, 1, 1);
                }

                Console.WriteLine($"File {fileName} was sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

            List<byte[]> GetContentSequence()
            {
                var messages = new List<byte[]>();
                for (var i = 0; i < message.Length; i += Size)
                {
                    messages.Add(message.Skip(i).Take(Size).ToArray());
                }

                return messages;
            }

            void SendMessage(IProducer<string, byte[]> producer, byte[] content, int position, int size)
            {
                var key = $"{fileName}||{position}||{size}";
                var messages = new Message<string, byte[]>
                {
                    Key = key,
                    Value = content
                };

                producer.ProduceAsync(Topic, messages).GetAwaiter().GetResult();
            }
        }
    }
}
