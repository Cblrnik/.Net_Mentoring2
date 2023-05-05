using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Confluent.Kafka;

namespace MainProcessingService
{
    public class Consumer
    {
        private const string Topic = "test";
        private const string GroupId = "test_group";
        private const string BootstrapServers = "localhost:9092";

        public void StartAsync()
        {
            var config = new ConsumerConfig
            {
                GroupId = GroupId,
                BootstrapServers = BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest,
            };
            var transferredFiles = new Dictionary<string, List<byte>>();

            try
            {
                using var consumerBuilder = new ConsumerBuilder<string, byte[]>(config).Build();
                consumerBuilder.Subscribe(Topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    Console.WriteLine("Service started");
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume
                            (cancelToken.Token);

                        if (!(consumer is { Message: { } })) continue;

                        var message = consumer.Message;

                        var fileName = ParseKey(message.Key, out var size, out var position);

                        var content = message.Value;

                        if (!transferredFiles.ContainsKey(fileName))
                        {
                            transferredFiles.Add(fileName, new List<byte>(content));
                        }
                        else
                        {
                            transferredFiles[fileName] = new List<byte>(transferredFiles[fileName].Concat(content));
                        }

                        if (size == position)
                        {
                            var fileStream = File.Create($"D:\\Mentoring\\MainProcessingService\\bin\\Debug\\Test\\{fileName}");

                            var fileData = transferredFiles.GetValueOrDefault(fileName)?.ToArray();
                            if (fileData != null) fileStream.Write(fileData, 0, fileData.Length);
                            fileStream.Close();
                            Console.WriteLine($"File {message.Key} was delivered");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ParseKey(string key, out int size, out int position)
        {
            var chunk = key.Split("||");

            size = int.Parse(chunk[2]);
            position = int.Parse(chunk[1]);

            return chunk[0];
        }
    }
}

