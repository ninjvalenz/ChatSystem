using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Handlers
{
    public class AssignChatHandler
    {
        private readonly ChatDbContext _context;

        public AssignChatHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task Handle()
        {
            var pendingChats = await _context.ChatQueue
                .Where(q => !q.Processed)
                .OrderBy(q => q.CreatedAt) 
                .ToListAsync();

            if (!pendingChats.Any()) return;

            var availableAgents = await _context.Agents
                .Where(a => a.CurrentChats < a.MaxChats)
                .OrderBy(a => a.Seniority) 
                .ToListAsync();

            foreach (var chat in pendingChats)
            {
                var agent = availableAgents.FirstOrDefault();
                if (agent == null) break; 

                chat.Processed = true;
                var chatSession = await _context.ChatSessions.FindAsync(chat.SessionId);
                chatSession.AssignedAgentId = agent.Id;

                agent.CurrentChats++;

                _context.ChatQueue.Update(chat);
                _context.ChatSessions.Update(chatSession);
                _context.Agents.Update(agent);

                await _context.SaveChangesAsync();
            }
        }
    }
}
