using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale;

/// <summary>
/// Represents immutable customer information as part of the External Identities pattern, 
/// containing denormalized customer data needed for sales operations.
/// </summary>
/// <param name="CustomerId">The unique identifier of the customer</param>
/// <param name="UserName">The customer's username</param>
/// <param name="Email">The customer's email address</param>
/// <param name="Phone">The customer's phone number</param>
/// <param name="Category">The customer's role/category, defaults to None</param>
public record CustomerInfo(
    Guid CustomerId,
    string UserName,
    string Email, 
    string Phone,
    UserRole Category = UserRole.None);
