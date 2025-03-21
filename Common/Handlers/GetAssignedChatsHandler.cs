using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Queries;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Handlers
{
    public class GetAssignedChatsHandler
    {
        private readonly ChatDbContext _context;

        public GetAssignedChatsHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<List<object>> Handle(GetAssignedChatsQuery query)
        {
            var assignedChats = await _context.ChatSessions
            .Where(c => c.AssignedAgentId != null && c.IsActive)
            .Join(
                _context.Agents,
                chat => chat.AssignedAgentId,
                agent => agent.Id,
                (chat, agent) => new
                {
                    chat.SessionId,
                    chat.AssignedAgentId,
                    AgentName = agent.Name,
                    Status = "Assigned"
                }
            )
            .ToListAsync();

            return assignedChats.Cast<object>().ToList();
        }
    }
}
