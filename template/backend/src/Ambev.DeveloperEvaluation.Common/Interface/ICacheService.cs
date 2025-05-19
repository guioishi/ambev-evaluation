namespace Ambev.DeveloperEvaluation.Common.Interface;

/// <summary>
/// Defines the contract for cache operations in the application.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a cached item by its key.
    /// </summary>
    /// <typeparam name="T">The type of the cached item</typeparam>
    /// <param name="key">The key of the cached item</param>
    /// <returns>The cached item if found; otherwise, null</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Stores an item in the cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the item to cache</typeparam>
    /// <param name="key">The key to store the item under</param>
    /// <param name="value">The item to cache</param>
    /// <param name="expiration">Optional expiration time for the cached item</param>
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    /// Removes an item from the cache by its key.
    /// </summary>
    /// <param name="key">The key of the item to remove</param>
    Task RemoveAsync(string key);
}
