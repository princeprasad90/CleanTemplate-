namespace CleanTemplate.Domain.Events;

public record ProductApproved(Guid ProductId, DateTime OccurredOn) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
