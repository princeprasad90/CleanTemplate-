namespace CleanTemplate.Domain.Events;

public record ProductSubmitted(Guid ProductId, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
