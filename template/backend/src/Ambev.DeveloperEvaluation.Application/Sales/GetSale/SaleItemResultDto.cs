namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record SaleItemResultDto(
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalPrice);
