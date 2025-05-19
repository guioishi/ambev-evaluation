using Ambev.DeveloperEvaluation.Common.Settings;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.MongoDB;
using Ambev.DeveloperEvaluation.ORM.MongoDB.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

/// <summary>
/// Provides MongoDB dependency injection configuration
/// </summary>
public static class MongoDependencyInjection
{
    /// <summary>
    /// Configures MongoDB services in the dependency injection container
    /// </summary>
    /// <param name="services">The service collection to add MongoDB services to</param>
    /// <param name="configuration">The configuration containing MongoDB settings</param>
    /// <returns>The service collection for chaining</returns>
    /// <exception cref="InvalidOperationException">Thrown when MongoDB settings are missing from configuration</exception>
    /// <remarks>
    /// This method:
    /// 1. Registers MongoDB class maps for BSON serialization
    /// 2. Loads and validates MongoDB settings from configuration
    /// 3. Configures MongoDB client and database connections
    /// 4. Registers repositories
    /// 
    /// Required configuration section in appsettings.json:
    /// {
    ///   "MongoDBSettings": {
    ///     "ConnectionString": "mongodb://connection-string",
    ///     "DatabaseName": "database-name"
    ///   }
    /// }
    /// </remarks>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        // Register MongoDB class mappings for BSON serialization
        MongoClassMaps.RegisterClassMaps();

        // Load MongoDB settings from configuration
        var mongoSettings = configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();

        if (mongoSettings is null)
        {
            throw new InvalidOperationException("MongoDBSettings configuration section is missing");
        }

        // Validate settings
        mongoSettings.Validate();

        // Make settings available for injection
        services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSettings"));

        // Register MongoDB client as singleton
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(mongoSettings.ConnectionString));

        // Register database instance with scoped lifetime
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoSettings.DatabaseName);
        });

        // Register repositories
        services.AddScoped<ISaleEventRepository>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return new SaleEventMongoRepository(database, mongoSettings);
        });

        return services;
    }
}
