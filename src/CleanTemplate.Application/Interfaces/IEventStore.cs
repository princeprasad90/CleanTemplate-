using CleanTemplate.Domain.Events;

namespace CleanTemplate.Application.Interfaces;

public interface IEventStore
{
    Task SaveAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default);
    Task<IEnumerable<IEvent>> GetAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task SaveSnapshotAsync<T>(Guid aggregateId, T snapshot, CancellationToken cancellationToken = default);
    Task<(T? Snapshot, IEnumerable<IEvent> Events)> GetWithSnapshotAsync<T>(Guid aggregateId, CancellationToken cancellationToken = default);
}
