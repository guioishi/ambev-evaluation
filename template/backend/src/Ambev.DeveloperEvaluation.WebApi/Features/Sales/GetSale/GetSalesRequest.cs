namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSalesRequest(
    DateTime? FromDate,
    DateTime? ToDate,
    Guid? BranchId);
