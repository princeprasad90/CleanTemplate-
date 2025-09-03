namespace CleanTemplate.Domain.Events;

public record OrderCreated(Guid OrderId, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
