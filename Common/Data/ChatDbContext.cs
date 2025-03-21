using Microsoft.EntityFrameworkCore;
using Common.Models;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<ChatQueue> ChatQueue { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatQueue>().HasKey(q => q.SessionId);
        modelBuilder.Entity<ChatSession>().HasKey(cs => cs.SessionId);
    }
}