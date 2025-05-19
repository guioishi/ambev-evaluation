namespace Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

public record BranchInfoDto(
    Guid BranchId,
    string BranchName
)
{
    // mapper
    public BranchInfoDto() : this(default, "")
    {
    }
}
