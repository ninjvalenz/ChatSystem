using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Handlers;


namespace PollingService.Service
{
    public class PollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PollingService> _logger;

        public PollingService(IServiceScopeFactory scopeFactory, ILogger<PollingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                    var assignChatHandler = scope.ServiceProvider.GetRequiredService<AssignChatHandler>();

                    await AssignChats(assignChatHandler);
                    await MonitorInactiveChats(context);
                }

                await Task.Delay(1000, stoppingToken); 
            }
        }

        private async Task AssignChats(AssignChatHandler assignChatHandler)
        {
            try
            {
                await assignChatHandler.Handle(); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning chats: {ex.Message}");
            }
        }

        private async Task MonitorInactiveChats(ChatDbContext context)
        {
            var inactiveChats = await context.ChatSessions
                .Where(c => c.IsActive && !context.ChatQueue.Any(q => q.SessionId == c.SessionId))
                .ToListAsync();

            foreach (var chat in inactiveChats)
            {
                _logger.LogWarning($"Mark chdtt {chat.SessionId} as inactive.");
                chat.IsActive = false;
                context.ChatSessions.Update(chat);
            }

            await context.SaveChangesAsync();
        }
    }
}