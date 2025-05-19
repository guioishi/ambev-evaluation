namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;

public class GetProductResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ProductRatingResponse Rating { get; set; } = new();
}

public class ProductRatingResponse
{
    public double Rate { get; set; }
    public int Count { get; set; }
}
