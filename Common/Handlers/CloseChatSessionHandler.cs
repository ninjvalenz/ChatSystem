using System;
using System.Collections.Generic;
using System.Text;
using Common.Command;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Common.Handlers
{
    public class CloseChatSessionHandler
    {
        private readonly ChatDbContext _context;

        public CloseChatSessionHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CloseChatSessionCommand command)
        {
            var chatSession = await _context.ChatSessions
                .FirstOrDefaultAsync(c => c.SessionId == command.SessionId);

            if (chatSession == null)
            {
                return "Chat session not found.";
            }

            if (!chatSession.IsActive)
            {
                return "Chat session is already closed.";
            }

            chatSession.IsActive = false;

            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == chatSession.AssignedAgentId);
            if (agent != null)
            {
                agent.CurrentChats = Math.Max(0, agent.CurrentChats - 1);
                _context.Agents.Update(agent);
            }

            _context.ChatSessions.Update(chatSession);
            await _context.SaveChangesAsync();

            return "Chat session closed successfully.";
        }
    }
}
