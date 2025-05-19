namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record CreateSaleResponse(
    string SaleNumber,
    DateTime SaleDate,
    string BranchName,
    decimal TotalAmount,
    List<SaleItemResponse> Items);
