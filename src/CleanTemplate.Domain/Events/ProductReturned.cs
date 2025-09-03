namespace CleanTemplate.Domain.Events;

public record ProductReturned(Guid ProductId, string Comment, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
