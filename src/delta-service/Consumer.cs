using Confluent.Kafka;
using Confluent.Kafka.Extensions.Diagnostics;

namespace DeltaService;

public class Consumer
{
    public void Run(CancellationToken cancellationToken = default)
    {
        ConsumerConfig config = new()
        {
            BootstrapServers = "kafka:29092",
            GroupId = "group.delta",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Factory.StartNew(async () =>
        {
            using IConsumer<string, string>? consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("test.topic");

            Console.WriteLine("Start consumer loop");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await consumer.ConsumeWithInstrumentation((result, _) =>
                    {
                        if (result is not null)
                        {
                            Console.WriteLine($"{result.Message.Key} - {result.Message.Value}");
                        }

                        return Task.CompletedTask;
                    }, cancellationToken);
                }
                catch
                {
                    await Task.Delay(100, cancellationToken);
                }
            }
        }, cancellationToken);
    }
}