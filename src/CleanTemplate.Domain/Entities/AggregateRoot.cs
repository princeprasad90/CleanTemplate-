using CleanTemplate.Domain.Events;

namespace CleanTemplate.Domain.Entities;

public abstract class AggregateRoot
{
    private readonly List<IEvent> _events = new();
    public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

    protected void AddEvent(IEvent @event) => _events.Add(@event);

    public void ClearEvents() => _events.Clear();
}
