using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSalesQuery(
    DateTime? FromDate,
    DateTime? ToDate,
    Guid? BranchId) : IRequest<List<SaleResultDto>>
{
    // mapper
    public GetSalesQuery() : this(null, null, null)
    {
    }
}
