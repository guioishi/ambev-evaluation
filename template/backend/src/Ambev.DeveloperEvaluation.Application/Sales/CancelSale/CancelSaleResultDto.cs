namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleResultDto(
    string SaleNumber,
    bool IsCancelled,
    DateTime CancellationDate)
{
    // mapper
    public CancelSaleResultDto() : this(
        string.Empty,
        false,
        default)
    {
    }
}
