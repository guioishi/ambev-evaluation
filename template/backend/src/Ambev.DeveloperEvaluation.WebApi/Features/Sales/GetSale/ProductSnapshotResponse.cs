namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record ProductSnapshotResponse(Guid ProductId, string ProductName, string ProductCode)
{
    // mapper
    public ProductSnapshotResponse() : this(default, "", "")
    {
    }
}
