using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleCommand(
    Guid BranchId,
    string BranchName,
    List<SaleProductDto> Items) : IRequest<SaleResultDto>, IUserContextRequest
{
    // mapper
    public CreateSaleCommand() : this(Guid.Empty, string.Empty, [])
    {
    }

    public CustomerInfoDto Customer { get; set; }
}
