using Confluent.Kafka;

namespace AlphaService;

public class Producer
{
    public async Task SendMessage(string message)
    {
        ProducerConfig config = new()
        {
            BootstrapServers = "localhost:9092",
        };

        using IProducer<string, string>? producer = new ProducerBuilder<string, string>(config).Build();
        await producer.ProduceAsync("test.topic", new Message<string, string>
            { Key = Guid.NewGuid().ToString(), Value = message });
    }
}