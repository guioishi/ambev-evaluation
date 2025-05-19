namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Defines constant values for Redis pub/sub channel names used across the application
/// </summary>
public static class RedisChannels
{
    /// <summary>
    /// Channel for publishing sales-related events like created, modified, or cancelled sales
    /// </summary>
    public const string SaleEvents = "sale:events";

    /// <summary>
    /// Channel for publishing product-related events like created, modified, or cancelled product
    /// </summary>
    public const string ProductEvents = "product:events";

    /// <summary>
    /// Channel for broadcasting cache invalidation messages to all application instances
    /// </summary>
    public const string CacheInvalidation = "cache:invalidation";

    /// <summary>
    /// Channel for system-wide notifications and alerts
    /// </summary>
    public const string SystemNotifications = "system:notifications";
}
