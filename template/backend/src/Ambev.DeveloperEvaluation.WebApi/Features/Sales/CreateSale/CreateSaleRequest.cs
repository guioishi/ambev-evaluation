namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record CreateSaleRequest(
    Guid CustomerId,
    Guid BranchId,
    string BranchName,
    List<SaleProductRequest> Items);
