using CleanTemplate.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Infrastructure.EF;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

    public DbSet<EventEntity> Events => Set<EventEntity>();
    public DbSet<SnapshotEntity> Snapshots => Set<SnapshotEntity>();
}
