namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProducts;

public class GetProductsQueryResponse
{
    public List<GetProductsResponse> Products { get; set; } = new();
    public int TotalItems { get; set; }
}

public class GetProductsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public double RatingRate { get; set; }
    public int RatingCount { get; set; }
}
