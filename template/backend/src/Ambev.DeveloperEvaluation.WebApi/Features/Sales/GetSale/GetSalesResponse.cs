using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSalesResponse(
    string SaleNumber,
    DateTime SaleDate,
    CustomerInfoResponse Customer,
    BranchInfoResponse Branch,
    decimal TotalAmount,
    bool IsCancelled,
    List<SaleItemResponse> Items);
