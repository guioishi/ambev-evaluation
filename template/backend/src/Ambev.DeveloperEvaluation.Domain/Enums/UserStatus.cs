namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the possible states of a user account in the system
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// Default state when user status cannot be determined
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// User account is active and can access the system normally
    /// </summary>
    Active,

    /// <summary>
    /// User account is inactive (e.g., voluntary account closure)
    /// </summary>
    Inactive,

    /// <summary>
    /// User account has been temporarily suspended (e.g., due to policy violations)
    /// </summary>
    Suspended
}
