using Confluent.Kafka;

namespace DeltaService;

public class Consumer
{
    public void Run()
    {
        ConsumerConfig config = new()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "group.delta",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Factory.StartNew(() =>
        {
            using IConsumer<string, string>? consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("test.topic");
            while (true)
            {
                ConsumeResult<string, string>? result = consumer.Consume();
                Console.WriteLine($"{result.Message.Key} - {result.Message.Value}");
            }
        });
    }
}