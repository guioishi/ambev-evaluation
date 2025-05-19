namespace Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;

/// <summary>
/// Event raised when an entire sale is cancelled
/// </summary>
public class SaleCancelledEvent : IEvent
{
    /// <summary>
    /// Initializes a new instance of the SaleCancelledEvent
    /// </summary>
    /// <param name="saleId">Unique identifier of the cancelled sale</param>
    /// <param name="cancellationDate">Date and time when the sale was cancelled</param>
    /// <param name="cancelledBy">Identifier of the user who cancelled the sale</param>
    /// <param name="reason">Reason provided for cancelling the sale</param>
    public SaleCancelledEvent(Guid saleId, DateTime cancellationDate, Guid cancelledBy, string reason)
    {
        SaleId = saleId;
        CancellationDate = cancellationDate;
        CancelledBy = cancelledBy;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the cancelled sale
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Date and time when the sale was cancelled
    /// </summary>
    public DateTime CancellationDate { get; }

    /// <summary>
    /// Identifier of the user who cancelled the sale
    /// </summary>
    public Guid CancelledBy { get; }

    /// <summary>
    /// Reason provided for cancelling the sale
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// UTC timestamp when this event was created
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Type identifier for this event
    /// </summary>
    public string EventType => "SaleCancelled";
}
