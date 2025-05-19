using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleQuery(string SaleNumber)
    : IRequest<GetSaleResultDto>;
