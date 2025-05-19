namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record BranchInfoResponse(
    Guid BranchId,
    string BranchName)
{
    // mapper
    public BranchInfoResponse() : this(default, "")
    {
    }
}
