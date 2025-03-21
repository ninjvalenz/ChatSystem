using System;
using System.Collections.Generic;
using System.Text;
using Common.Command;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Common.Handlers
{
    public class CreateChatSessionHandler
    {
        private readonly ChatDbContext _context;

        public CreateChatSessionHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateChatSessionCommand command)
        {
            int agentCapacity = await CalculateAgentCapacity();
            int maxQueueSize = (int)(agentCapacity * 1.5);
            int currentQueueSize = await _context.ChatQueue.CountAsync(q => !q.Processed);

            if (currentQueueSize >= maxQueueSize)
            {
                if (IsOfficeHours())
                {
                    return await CreateChatSession(command, isOverflow: true);
                }
                else
                {
                    throw new InvalidOperationException("Chat queue is full, try again later.");
                }
            }

            return await CreateChatSession(command, isOverflow: false);
        }

        private async Task<Guid> CreateChatSession(CreateChatSessionCommand command, bool isOverflow)
        {
            var sessionId = Guid.NewGuid();

            var chatSession = new ChatSession
            {
                SessionId = sessionId,
                UserId = command.UserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.ChatSessions.Add(chatSession);

            var chatQueue = new ChatQueue
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                UserId = command.UserId,
                CreatedAt = DateTime.UtcNow,
                Processed = false,
                IsOverflow = isOverflow
            };

            _context.ChatQueue.Add(chatQueue);
            await _context.SaveChangesAsync();

            return sessionId;
        }

        private async Task<int> CalculateAgentCapacity()
        {
            var agents = await _context.Agents.ToListAsync();
            return agents.Sum(a => (int)(a.MaxChats * GetSeniorityMultiplier(a.Seniority)));
        }

        private double GetSeniorityMultiplier(string seniority)
        {
            return seniority switch
            {
                "Junior" => 0.4,
                "Mid-Level" => 0.6,
                "Senior" => 0.8,
                "Team Lead" => 0.5,
                _ => 0.4
            };
        }

        private bool IsOfficeHours()
        {
            var now = DateTime.UtcNow.TimeOfDay;
            return now >= TimeSpan.FromHours(8) && now <= TimeSpan.FromHours(18);
        }
    }
}