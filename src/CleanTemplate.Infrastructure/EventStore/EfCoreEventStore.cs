using System.Text.Json;
using CleanTemplate.Application.Interfaces;
using CleanTemplate.Domain.Events;
using CleanTemplate.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Infrastructure.EventStore;

public class EfCoreEventStore : IEventStore
{
    private readonly EventDbContext _context;

    public EfCoreEventStore(EventDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var entity = new EventEntity
            {
                Id = @event.Id,
                AggregateId = aggregateId,
                Type = @event.GetType().AssemblyQualifiedName!,
                Data = JsonSerializer.Serialize(@event, @event.GetType()),
                OccurredOn = @event.OccurredOn
            };
            _context.Events.Add(entity);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<IEvent>> GetAsync(Guid aggregateId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.OccurredOn)
            .ToListAsync(cancellationToken);

        var events = new List<IEvent>();
        foreach (var entity in entities)
        {
            var type = Type.GetType(entity.Type)!;
            var @event = (IEvent)JsonSerializer.Deserialize(entity.Data, type)!;
            events.Add(@event);
        }

        return events;
    }

    public async Task SaveSnapshotAsync<T>(Guid aggregateId, T snapshot, CancellationToken cancellationToken = default)
    {
        var entity = new SnapshotEntity
        {
            Id = Guid.NewGuid(),
            AggregateId = aggregateId,
            Type = snapshot!.GetType().AssemblyQualifiedName!,
            Data = JsonSerializer.Serialize(snapshot, snapshot.GetType()),
            CreatedOn = DateTime.UtcNow
        };
        _context.Snapshots.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(T? Snapshot, IEnumerable<IEvent> Events)> GetWithSnapshotAsync<T>(Guid aggregateId, CancellationToken cancellationToken = default)
    {
        var snapshotEntity = await _context.Snapshots
            .Where(s => s.AggregateId == aggregateId)
            .OrderByDescending(s => s.CreatedOn)
            .FirstOrDefaultAsync(cancellationToken);

        T? snapshot = default;
        DateTime? from = null;
        if (snapshotEntity != null)
        {
            var type = Type.GetType(snapshotEntity.Type)!;
            snapshot = (T)JsonSerializer.Deserialize(snapshotEntity.Data, type)!;
            from = snapshotEntity.CreatedOn;
        }

        var query = _context.Events.Where(e => e.AggregateId == aggregateId);
        if (from.HasValue)
        {
            query = query.Where(e => e.OccurredOn > from.Value);
        }

        var entities = await query
            .OrderBy(e => e.OccurredOn)
            .ToListAsync(cancellationToken);

        var events = new List<IEvent>();
        foreach (var entity in entities)
        {
            var type = Type.GetType(entity.Type)!;
            var @event = (IEvent)JsonSerializer.Deserialize(entity.Data, type)!;
            events.Add(@event);
        }

        return (snapshot, events);
    }
}
