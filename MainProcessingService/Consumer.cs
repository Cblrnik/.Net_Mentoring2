using System;
using System.IO;
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

            try
            {
                using var consumerBuilder = new ConsumerBuilder<string, byte[]>(config).Build();
                consumerBuilder.Subscribe(Topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume
                            (cancelToken.Token);

                        if (!(consumer is { Message: { } })) continue;

                        var message = consumer.Message;
                        var file = message.Value;

                        var fileStream = File.Create($"D:\\Mentoring\\MainProcessingService\\bin\\Debug\\Test\\{message.Key}");

                        fileStream.Write(file,
                            0, file.Length);

                        fileStream.Close();

                        Console.WriteLine($"File {message.Key} was delivered");
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
    }
}

