using System.Linq;
using System.Threading.Tasks;
using AgentService.Service.Interface;
using Microsoft.EntityFrameworkCore;

public class AgentAssignmentService : IAgentAssignmentService
{
    private readonly ChatDbContext _context;

    public AgentAssignmentService(ChatDbContext context)
    {
        _context = context;
    }

    public async Task AssignChats()
    {
        var pendingChat = await _context.ChatQueue
            .Where(q => !q.Processed)
            .OrderBy(q => q.CreatedAt)
            .FirstOrDefaultAsync();

        if (pendingChat == null) return;

        var availableAgent = await _context.Agents
            .Where(a => a.CurrentChats < a.MaxChats)
            .OrderBy(a => a.Seniority)
            .FirstOrDefaultAsync();

        if (availableAgent == null) return;

        pendingChat.Processed = true;
        availableAgent.CurrentChats++;

        _context.Entry(pendingChat).State = EntityState.Modified;
        _context.Entry(availableAgent).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}