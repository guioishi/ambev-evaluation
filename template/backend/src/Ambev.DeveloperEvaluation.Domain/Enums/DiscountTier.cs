namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents available discount tiers for sales items based on quantity thresholds.
/// The enum value represents the percentage discount.
/// </summary>
public enum DiscountTier
{
    /// <summary>
    /// No discount applied (for quantities less than 4)
    /// </summary>
    None = 0,

    /// <summary>
    /// 10% discount applied for quantities between 4-9 items
    /// </summary>
    TenPercent = 10,

    /// <summary>
    /// 20% discount applied for quantities between 10-20 items
    /// </summary>
    TwentyPercent = 20
}
