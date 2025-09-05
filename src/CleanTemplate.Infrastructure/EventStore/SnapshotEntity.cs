namespace CleanTemplate.Infrastructure.EventStore;

public class SnapshotEntity
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = default!;
    public string State { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
}
