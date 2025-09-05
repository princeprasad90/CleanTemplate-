namespace CleanTemplate.Application.Interfaces;

/// <summary>
/// Represents a snapshot of an aggregate with its type, identifier and state.
/// </summary>
/// <typeparam name="TState">Type of the aggregate state stored in the snapshot.</typeparam>
public record Snapshot<TState>(Guid AggregateId, string AggregateType, TState State);
