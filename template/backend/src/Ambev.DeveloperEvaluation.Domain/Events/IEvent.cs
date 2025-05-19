namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Defines the contract for domain events in the system
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Unique identifier for the event instance
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// UTC timestamp indicating when the event occurred
    /// </summary>
    DateTime OccurredOn { get; }

    /// <summary>
    /// String identifier representing the type of event
    /// </summary>
    string EventType { get; }
}
