using System.Threading.Tasks;
using System;
using ChatAPI.Service.Interface;
using MessageBroker.Interface;
using ChatAPI.Model;

public class ChatService : IChatService
{
    private readonly ChatDbContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public ChatService(ChatDbContext context, IMessagePublisher messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }

    public async Task<ChatResponseDto> CreateChatSession(ChatRequestDto request)
    {
        var session = new ChatSession
        {
            SessionId = Guid.NewGuid(),
            UserId = request.UserID,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ChatSessions.Add(session);
        await _context.SaveChangesAsync();

        await _messagePublisher.PublishAsync("chat.created", session);

        return new ChatResponseDto { IsSuccess = true, SessionId = session.SessionId };
    }
}