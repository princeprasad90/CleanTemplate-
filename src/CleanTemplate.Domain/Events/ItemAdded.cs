namespace CleanTemplate.Domain.Events;

public record ItemAdded(Guid OrderId, string Product, int Quantity, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
