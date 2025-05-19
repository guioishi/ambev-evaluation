using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleCommand(string SaleNumber)
    : IRequest<CancelSaleResultDto>;
