namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Defines the authorization roles available in the system with increasing levels of access
/// </summary>
public enum UserRole
{
    /// <summary>
    /// No specific role assigned, typically used as a default or for unauthenticated users
    /// </summary>
    None = 0,

    /// <summary>
    /// Standard customer role with basic access to shopping and order management
    /// </summary>
    Customer,    

    /// <summary>
    /// Branch manager role with access to sales reports and staff management
    /// </summary>
    Manager,

    /// <summary>
    /// System administrator with full access to all features and settings
    /// </summary>
    Admin,
}
