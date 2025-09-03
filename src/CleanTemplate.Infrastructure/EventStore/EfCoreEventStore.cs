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
}
