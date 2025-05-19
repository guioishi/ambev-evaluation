using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Application.Common;

/// <summary>
/// Interface for requests that require customer context information.
/// Used in conjunction with UserContextBehavior to automatically populate
/// customer information from the authentication token.
/// </summary>
public interface IUserContextRequest
{
    /// <summary>
    /// Gets or sets customer information for the authenticated user making the request.
    /// This property is automatically populated by UserContextBehavior using the authentication token.
    /// </summary>
    CustomerInfoDto Customer { get; set; }
}
