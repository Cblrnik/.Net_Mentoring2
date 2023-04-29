namespace MainProcessingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var consumer = new Consumer();
            consumer.StartAsync();
        }
    }
}
