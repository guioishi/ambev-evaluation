using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Product;

/// <summary>
/// Represents a product in the catalog with quantity discount rules
/// </summary>
public class Product : BaseEntity
{
    // Product properties
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string ImageUrl { get; init; }
    public ProductRating Rating { get; init; }

    /// <summary>
    /// Factory method for product creation
    /// </summary>
    public static Product Create(
        string title,
        decimal price,
        string description,
        string category,
        string imageUrl,
        double rate,
        int count)
    {
        var product = new Product
        {
            Title = title,
            Price = price,
            Description = description,
            Category = category,
            ImageUrl = imageUrl,
            Rating = ProductRating.Create(rate, count)
        };

        return product;
    }
}

/// <summary>
/// Value Object representing product rating
/// </summary>
public class ProductRating
{
    public double Rate { get; }
    public int Count { get; }

    private ProductRating(double rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    public ProductRating()
    {
    }

    public static ProductRating Create(double rate, int count)
    {
        if (rate is < 0 or > 5)
            throw new DomainException("Rating must be between 0 and 5");

        if (count < 0)
            throw new DomainException("Rating count cannot be negative");

        return new ProductRating(rate, count);
    }
}
