using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale;

/// <summary>
/// Represents a sale transaction in the system
/// </summary>
public class Sale : BaseEntity
{
    private const int DiscountThreshold1 = 4;
    private const int DiscountThreshold2 = 10;
    private const int MaxQuantityPerSale = 20;

    public string SaleNumber { get; init; }
    public DateTime SaleDate { get; init; }
    public CustomerInfo Customer { get; init; }
    public BranchInfo Branch { get; init; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }

    private readonly List<SaleProduct> _saleProducts = [];

    /// <summary>
    /// Read-only collection of products in this sale
    /// </summary>
    public IReadOnlyCollection<SaleProduct> SaleProducts => _saleProducts.AsReadOnly();

    // Thread-safe sale number generation
    private static long _lastTimestamp;
    private static int _counter;

    /// <summary>
    /// Creates a new sale
    /// </summary>
    public static Sale Create(
        CustomerInfo customer,
        Guid branchId,
        string branchName)
    {
        return new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = GenerateDistributedSaleNumber(),
            SaleDate = DateTime.UtcNow,
            Customer = customer,
            Branch = new BranchInfo(branchId, branchName),
            IsCancelled = false
        };
    }

    /// <summary>
    /// Add products to sale and validates it
    /// </summary>
    public void AddProduct(Guid productId, int quantity)
    {
        CheckQuantity(quantity);

        var existingItem = _saleProducts.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            CheckQuantity(existingItem.Quantity + quantity);
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new SaleProduct { ProductId = productId, Quantity = quantity };
            _saleProducts.Add(newItem);
        }

        MarkAsUpdated();
    }

    /// <summary>
    /// Updates the quantity of this item and recalculates the discount tier
    /// </summary>
    /// <param name="quantity">New quantity (must be 20 or less)</param>
    /// <exception cref="DomainException">Thrown when quantity exceeds 20 items</exception>
    public void CheckQuantity(int quantity)
    {
        switch (quantity)
        {
            case <= 0:
                throw new DomainException("Quantity must be greater than zero");
            case > MaxQuantityPerSale:
                throw new DomainException("Cannot add more than 20 units of the same product");
        }
    }

    /// <summary>
    /// Calculates the total price for a given quantity including discounts
    /// </summary>
    public void CalculateTotalPrice(decimal price, int quantity)
    {
        var discountTier = quantity switch
        {
            >= DiscountThreshold2 => 0.20m,
            >= DiscountThreshold1 => 0.10m,
            _ => 0m
        };

        TotalAmount += price * quantity * (1 - discountTier);
    }

    /// <summary>
    /// Cancels the sale
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            throw new DomainException("Sale is already cancelled");

        IsCancelled = true;
        MarkAsUpdated();
    }

    private static string GenerateDistributedSaleNumber()
    {
        var now = DateTime.UtcNow;
        var timestamp = now.Ticks / 10000;

        long lastTs;
        int count;

        do
        {
            lastTs = Volatile.Read(ref _lastTimestamp);
            count = (lastTs == timestamp)
                ? Interlocked.Increment(ref _counter)
                : (Interlocked.Exchange(ref _counter, 1), 1).Item2;
        } while (Interlocked.CompareExchange(ref _lastTimestamp, timestamp, lastTs) != lastTs);

        return $"SALE-{now:yyyyMMdd-HHmmssfff}-{count:D2}";
    }
}

/// <summary>
/// Request for adding products to a sale
/// </summary>
public class SaleProduct
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public Sale Sale { get; set; }

    public Product.Product Product { get; set; }
}
