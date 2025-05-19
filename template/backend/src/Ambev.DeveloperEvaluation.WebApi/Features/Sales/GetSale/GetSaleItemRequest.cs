namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record GetSaleItemRequest(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount);
