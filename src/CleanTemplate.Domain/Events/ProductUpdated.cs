namespace CleanTemplate.Domain.Events;

public record ProductUpdated(Guid ProductId, string Name, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
