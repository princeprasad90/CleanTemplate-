namespace CleanTemplate.Domain.Events;

public record ProductCreated(Guid ProductId, string Name, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
