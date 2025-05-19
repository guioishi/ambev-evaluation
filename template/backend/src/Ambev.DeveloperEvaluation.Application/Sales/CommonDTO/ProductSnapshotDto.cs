namespace Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

public record ProductSnapshotDto(Guid ProductId, string ProductName, string ProductCode)
{
    // mapper
    public ProductSnapshotDto() : this(default, "", "")
    {
    }
}
