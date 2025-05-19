namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ProductRatingDto Rating { get; set; } = new();
}

public class ProductRatingDto
{
    public double Rate { get; set; }
    public int Count { get; set; }
}
