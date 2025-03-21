using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using MessageBroker.Interface;

public class KafkaMessagePublisher : IMessagePublisher
{
    private readonly IProducer<string, string> _producer;

    public KafkaMessagePublisher(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(string topic, object message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<string, string> { Value = jsonMessage });
    }
}

