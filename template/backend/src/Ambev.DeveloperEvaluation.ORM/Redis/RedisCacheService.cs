using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.Interface;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.ORM.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(
        IConnectionMultiplexer redis,
        JsonSerializerOptions options,
        ILogger<RedisCacheService> logger)
    {
        _db = redis.GetDatabase();
        _serializerOptions = options;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            _logger.LogDebug("Attempting to get value from cache for key: {Key}", key);

            var value = await _db.StringGetAsync(key);
            if (!value.HasValue)
            {
                _logger.LogDebug("Cache miss for key: {Key}", key);
                return default;
            }

            var jsonString = value.ToString();
            _logger.LogDebug("Raw value from Redis: {Value}", jsonString);

            try
            {
                var result = JsonSerializer.Deserialize<T>(jsonString, _serializerOptions);
                if (result is null)
                {
                    _logger.LogWarning("Deserialization resulted in null for key: {Key}", key);
                    return default;
                }

                var properties = typeof(T).GetProperties();
                var hasNullProperties = properties.Any(p => p.GetValue(result) == null);

                if (hasNullProperties)
                {
                    _logger.LogWarning("Deserialized object has null properties for key: {Key}", key);
                    return default;
                }

                _logger.LogDebug("Successfully deserialized object of type {Type}: {@Result}", typeof(T).Name, result);
                return result;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error for key {Key}. JSON: {Json}", key, jsonString);
                return default;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving value from cache for key: {Key}", key);
            return default;
        }
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            _logger.LogDebug("Attempting to set cache value for key: {Key}, Expiration: {Expiration}",
                key, expiration);

            var serialized = JsonSerializer.Serialize(value, _serializerOptions);
            _logger.LogDebug("Serialized value: {SerializedValue}", serialized);

            var result = await _db.StringSetAsync(key, serialized, expiration);

            if (result)
                _logger.LogDebug("Successfully set cache value for key: {Key}", key);
            else
                _logger.LogWarning("Failed to set cache value for key: {Key}", key);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
            return false;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            _logger.LogDebug("Attempting to remove value from cache for key: {Key}", key);

            var result = await _db.KeyDeleteAsync(key);

            if (result)
                _logger.LogDebug("Successfully removed value from cache for key: {Key}", key);
            else
                _logger.LogWarning("Key not found in cache: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing value from cache for key: {Key}", key);
        }
    }
}
