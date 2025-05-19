namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record CustomerInfoResponse(
    Guid CustomerId,
    string UserName,
    string Email,
    string Phone,
    string Category)
{
    // mapper
    public CustomerInfoResponse() : this(default, "", "", "", "0")
    {
    }
}
