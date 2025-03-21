using System.Threading.Tasks;

namespace QueueService.Service.Interface
{
    public interface IQueueService
    {
        Task EnqueueChat(ChatSession chat);
        Task<ChatSession?> DequeueChat();
    }
}
