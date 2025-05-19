using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;

namespace Ambev.DeveloperEvaluation.Application.Services.Interface;

/// <summary>
/// Defines the contract for retrieving customer information.
/// </summary>
public interface ICustomerInfoService
{
    /// <summary>
    /// Retrieves customer information asynchronously by customer ID.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer</param>
    /// <returns>A task containing the customer information DTO if found</returns>
    /// <exception cref="KeyNotFoundException">Thrown when customer is not found</exception>
    Task<CustomerInfoDto> GetCustomerInfoAsync(Guid customerId);
}
