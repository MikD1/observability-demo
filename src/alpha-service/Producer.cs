using Confluent.Kafka;
using Confluent.Kafka.Extensions.Diagnostics;

namespace AlphaService;

public class Producer
{
    public async Task SendMessage(string message)
    {
        ProducerConfig config = new()
        {
            BootstrapServers = "kafka:29092"
        };

        using IProducer<string, string> producer =
            new ProducerBuilder<string, string>(config).BuildWithInstrumentation();
        await producer.ProduceAsync("test.topic", new Message<string, string>
            { Key = Guid.NewGuid().ToString(), Value = message });
    }
}