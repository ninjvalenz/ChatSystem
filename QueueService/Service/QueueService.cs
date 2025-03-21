using QueueService.Service.Interface;
using System.Threading.Tasks;
using System;
using Common.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QueueService.Service
{
    public class QueueService : IQueueService
    {
        private readonly ChatDbContext _context;

        public QueueService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task EnqueueChat(ChatSession chat)
        {
            var queueItem = new ChatQueue
            {
                SessionId = chat.SessionId,
                UserId = chat.UserId,
                CreatedAt = DateTime.UtcNow,
                Processed = false
            };

            _context.ChatQueue.Add(queueItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ChatSession?> DequeueChat()
        {
            var queueItem = await _context.ChatQueue
                .Where(q => !q.Processed)
                .OrderBy(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            if (queueItem == null) return null;

            queueItem.Processed = true;
            await _context.SaveChangesAsync();

            return await _context.ChatSessions.FindAsync(queueItem.SessionId);
        }
    }
}
