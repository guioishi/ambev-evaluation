namespace Ambev.DeveloperEvaluation.Domain.Events.Product;

/// <summary>
/// Event raised when a new product is created in the system
/// </summary>
public class ProductsAddedToSaleEvent : IEvent
{
    /// <summary>
    /// Initializes a new instance of the productCreatedEvent
    /// </summary>
    /// <param name="productId">Unique identifier of the newly created product</param>
    /// <param name="createdDate">Date and time when the product was created</param>
    public ProductsAddedToSaleEvent(Guid productId, DateTime createdDate, int quantity)
    {
        ProductId = productId;
        CreatedDate = createdDate;
        OccurredOn = DateTime.UtcNow;
        Quantity = quantity;
    }

    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    public Guid EventId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the newly created product
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Quantity of products
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Date and time when the product was created
    /// </summary>
    public DateTime CreatedDate { get; }

    /// <summary>
    /// UTC timestamp when this event was created
    /// </summary>
    public DateTime OccurredOn { get; set; }

    /// <summary>
    /// Type identifier for this event
    /// </summary>
    public string EventType => "ProductCreated";
}
