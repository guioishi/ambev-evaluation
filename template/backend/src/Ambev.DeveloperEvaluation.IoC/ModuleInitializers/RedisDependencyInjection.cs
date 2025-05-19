using System.Text.Json;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.ORM.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

/// <summary>
/// Provides extension methods for dependency injection configuration
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures Redis connection and services in the dependency injection container
    /// </summary>
    /// <param name="services">The service collection to add Redis services to</param>
    /// <param name="config">The configuration containing Redis connection settings</param>
    /// <returns>The service collection for chaining</returns>
    /// <remarks>
    /// This method:
    /// 1. Creates a singleton Redis connection using the connection string from configuration
    /// 2. Registers the IConnectionMultiplexer interface with the Redis connection
    /// 3. Registers the ICacheService interface with the RedisCacheService implementation
    /// 
    /// The Redis connection string should be configured in appsettings.json under "Redis:ConnectionString"
    /// </remarks>
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        var redis = ConnectionMultiplexer.Connect(config["Redis:ConnectionString"]);

        services.AddSingleton<IConnectionMultiplexer>(redis);
        services.AddSingleton(options);
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
