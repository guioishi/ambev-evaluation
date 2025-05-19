namespace Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

public record CustomerInfoDto(
    Guid CustomerId,
    string UserName,
    string Email,
    string Phone,
    string Category)
{
    // mapper
    public CustomerInfoDto() : this(default, "", "", "", "0")
    {
    }
}
