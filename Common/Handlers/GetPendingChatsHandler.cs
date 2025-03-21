using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Queries;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Handlers
{
    public class GetPendingChatsHandler
    {
        private readonly ChatDbContext _context;

        public GetPendingChatsHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Handle(GetPendingChatsQuery query)
        {
            var pendingChats = await _context.ChatQueue
                .Where(q => !q.Processed)  
                .Join(
                    _context.ChatSessions,  
                    queue => queue.SessionId,
                    session => session.SessionId,
                    (queue, session) => new
                    {
                        queue.Id, 
                        queue.SessionId,
                        queue.UserId,
                        session.IsActive,
                        queue.CreatedAt,
                        Status = "Queued"
                    }
                )
                .ToListAsync();

            return pendingChats.Cast<object>().ToList(); 
        }
    }
}
