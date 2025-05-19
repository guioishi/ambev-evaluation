using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.Common.HealthChecks;

/// <summary>
/// Health check implementation for MongoDB connection monitoring
/// </summary>
public class MongoDbHealthCheck : IHealthCheck
{
    private readonly string _connectionString;
    private readonly string _databaseName;
    private readonly TimeSpan _timeout;

    /// <summary>
    /// Initializes a new instance of the MongoDB health check
    /// </summary>
    /// <param name="connectionString">MongoDB connection string</param>
    /// <param name="databaseName">Name of the database to check (defaults to "admin")</param>
    /// <param name="timeout">Maximum time to wait for response (defaults to 3 seconds)</param>
    public MongoDbHealthCheck(string connectionString, string databaseName = "admin", TimeSpan? timeout = null)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        _timeout = timeout ?? TimeSpan.FromSeconds(3);
    }

    /// <summary>
    /// Performs the health check for MongoDB connection
    /// </summary>
    /// <param name="context">Context containing health check execution information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// HealthCheckResult indicating:
    /// - Healthy: When successfully connected and pinged the database
    /// - Unhealthy: When connection fails, authentication fails, or operation times out
    /// </returns>
    /// <remarks>
    /// The check performs a lightweight ping command to verify:
    /// - Connection can be established
    /// - Authentication is successful
    /// - Database responds within timeout period
    /// </remarks>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Create timeout-aware cancellation token
            using var timeoutCancellation = new CancellationTokenSource(_timeout);
            using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken, timeoutCancellation.Token);

            // Attempt database connection and ping
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var pingCommand = new BsonDocument("ping", 1);
            await database.RunCommandAsync<BsonDocument>(
                pingCommand,
                cancellationToken: linkedCancellation.Token);

            return HealthCheckResult.Healthy();
        }
        catch (MongoAuthenticationException authEx)
        {
            return HealthCheckResult.Unhealthy(
                "Failed to authenticate",
                authEx);
        }
        catch (TimeoutException timeoutEx)
        {
            return HealthCheckResult.Unhealthy(
                $"Timed out (limit: {_timeout.TotalSeconds}s)",
                timeoutEx);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Connection failed",
                ex);
        }
    }
}
