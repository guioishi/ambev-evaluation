namespace Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;

/// <summary>
/// Event raised when an existing sale is modified
/// </summary>
public class SaleModifiedEvent : IEvent
{
    /// <summary>
    /// Initializes a new instance of the SaleModifiedEvent
    /// </summary>
    /// <param name="saleId">Unique identifier of the modified sale</param>
    /// <param name="modifiedDate">Date and time when the sale was modified</param>
    /// <param name="modifiedBy">Username or identifier of who modified the sale</param>
    /// <param name="changeDetails">Description of what was modified in the sale</param>
    public SaleModifiedEvent(Guid saleId, DateTime modifiedDate, string modifiedBy, string changeDetails)
    {
        SaleId = saleId;
        ModifiedDate = modifiedDate;
        ModifiedBy = modifiedBy;
        ChangeDetails = changeDetails;
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the modified sale
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Date and time when the sale was modified
    /// </summary>
    public DateTime ModifiedDate { get; }

    /// <summary>
    /// Username or identifier of who modified the sale
    /// </summary>
    public string ModifiedBy { get; }

    /// <summary>
    /// Description of what was modified in the sale
    /// </summary>
    public string ChangeDetails { get; }

    /// <summary>
    /// UTC timestamp when this event was created
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Type identifier for this event
    /// </summary>
    public string EventType => "SaleModified";
}
