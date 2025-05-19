namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record SaleProductRequest(
    Guid ProductId,
    int Quantity)
{
    // mapper
    public SaleProductRequest() : this(default, 0) { }
}
