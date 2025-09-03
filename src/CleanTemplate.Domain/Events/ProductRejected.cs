namespace CleanTemplate.Domain.Events;

public record ProductRejected(Guid ProductId, string Comment, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
