using System.Threading.Tasks;

namespace MessageBroker.Interface
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string topic, object message);
    }
}