using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record SaleResultDto(
    string SaleNumber,
    DateTime SaleDate,
    CustomerInfoDto Customer,
    BranchInfoDto Branch,
    decimal TotalAmount,
    bool IsCancelled,
    List<GetSaleProductDto> Items)
{
    // Empty constructor for mapper
    public SaleResultDto()
        : this("", DateTime.MinValue, new CustomerInfoDto(), new BranchInfoDto(), 0m, false, new List<GetSaleProductDto>())
    {
    }
}
