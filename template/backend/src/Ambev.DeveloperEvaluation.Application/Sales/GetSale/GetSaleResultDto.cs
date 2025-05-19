using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleResultDto(
    string SaleNumber,
    DateTime SaleDate,
    CustomerInfoDto Customer,
    BranchInfoDto Branch,
    decimal TotalAmount,
    bool IsCancelled,
    List<GetSaleProductDto> Items)
{
    // mapper
    public GetSaleResultDto() : this(
        string.Empty,
        default,
        new CustomerInfoDto(Guid.Empty, string.Empty, string.Empty, string.Empty, "0"),
        new BranchInfoDto(Guid.Empty, string.Empty),
        default,
        default,
        [])
    {
    }
}
