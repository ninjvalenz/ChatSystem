using System.Threading.Tasks;
using ChatAPI.Model;

namespace ChatAPI.Service.Interface
{
    public interface IChatService
    {
        Task<ChatResponseDto> CreateChatSession(ChatRequestDto request);
    }
}