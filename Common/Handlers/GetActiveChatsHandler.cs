using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Handlers
{
    public class GetActiveChatsHandler
    {
        private readonly ChatDbContext _context;

        public GetActiveChatsHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatSession>> Handle(GetActiveChatsQuery query)
        {
            return await _context.ChatSessions
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }

}
