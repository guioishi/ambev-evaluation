using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale;

/// <summary>
/// Represents an immutable snapshot of product information at the time of sale,
/// following the External Identities pattern with denormalized product data.
/// </summary>
public record ProductSnapshot
{
    /// <summary>
    /// The unique identifier of the original product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The display name of the product
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The product's identifying code/SKU
    /// </summary> 
    public string Code { get; set; }

    /// <summary>
    /// Creates a new product snapshot with validation
    /// </summary>
    /// <param name="productId">Unique identifier of the product</param>
    /// <param name="name">Product name (required, max 100 characters)</param>
    /// <param name="code">Product code/SKU</param>
    /// <exception cref="DomainException">Thrown when name is empty or too long</exception>
    public ProductSnapshot(Guid productId, string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty");

        if (name.Length > 100)
            throw new DomainException("Product name too long");

        ProductId = productId;
        Name = name;
        Code = code;
    }
}
