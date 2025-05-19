namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsQueryResult
{
    public List<ProductListItemDto> Products { get; set; } = new();
    public int TotalItems { get; set; }
}

public class ProductListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public double RatingRate { get; set; }
    public int RatingCount { get; set; }
}
