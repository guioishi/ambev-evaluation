namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleResponse(
    string SaleNumber,
    DateTime SaleDate,
    CustomerInfoResponse Customer,
    BranchInfoResponse Branch,
    decimal TotalAmount,
    bool IsCancelled,
    List<GetSaleItemResponse> Items);
