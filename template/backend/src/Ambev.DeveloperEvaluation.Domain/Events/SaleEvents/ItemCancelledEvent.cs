namespace Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;

/// <summary>
/// Event raised when an item is cancelled within a sale
/// </summary>
public class ItemCancelledEvent : IEvent
{
    /// <summary>
    /// Initializes a new instance of the ItemCancelledEvent
    /// </summary>
    /// <param name="saleId">Unique identifier of the sale containing the cancelled item</param>
    /// <param name="itemId">Unique identifier of the cancelled item</param>
    /// <param name="quantity">Quantity of items being cancelled</param>
    /// <param name="cancellationDate">Date and time when the cancellation occurred</param>
    /// <param name="cancelledBy">Username or identifier of the person who performed the cancellation</param>
    /// <param name="reason">Reason provided for the cancellation</param>
    public ItemCancelledEvent(Guid saleId, Guid itemId, int quantity, DateTime cancellationDate, string cancelledBy,
        string reason)
    {
        SaleId = saleId;
        ItemId = itemId;
        Quantity = quantity;
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
    /// Identifier of the sale containing the cancelled item
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Identifier of the cancelled item
    /// </summary>
    public Guid ItemId { get; }

    /// <summary>
    /// Quantity of items being cancelled
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Date and time when the cancellation occurred
    /// </summary>
    public DateTime CancellationDate { get; }

    /// <summary>
    /// Username or identifier of the person who performed the cancellation
    /// </summary>
    public string CancelledBy { get; }

    /// <summary>
    /// Reason provided for the cancellation
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// UTC timestamp when this event was created
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Type identifier for this event
    /// </summary>
    public string EventType => "ItemCancelled";
}
