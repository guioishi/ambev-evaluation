namespace Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;

/// <summary>
/// Event raised when a new sale is created in the system
/// </summary>
public class SaleCreatedEvent : IEvent
{
    /// <summary>
    /// Initializes a new instance of the SaleCreatedEvent
    /// </summary>
    /// <param name="saleId">Unique identifier of the newly created sale</param>
    /// <param name="createdDate">Date and time when the sale was created</param>
    public SaleCreatedEvent(Guid saleId, DateTime createdDate)
    {
        SaleId = saleId;
        CreatedDate = createdDate;
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    public Guid EventId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the newly created sale
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Date and time when the sale was created
    /// </summary>
    public DateTime CreatedDate { get; }

    /// <summary>
    /// UTC timestamp when this event was created
    /// </summary>
    public DateTime OccurredOn { get; set; }

    /// <summary>
    /// Type identifier for this event
    /// </summary>
    public string EventType => "SaleCreated";
}
