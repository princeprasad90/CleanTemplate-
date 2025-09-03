namespace CleanTemplate.Domain.Events;

public record ProductDeleted(Guid ProductId, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
