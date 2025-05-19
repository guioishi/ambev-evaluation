namespace Ambev.DeveloperEvaluation.Common.Settings;

/// <summary>
/// Configuration settings for Redis connections
/// </summary>
public class RedisSettings
{
    /// <summary>
    /// Redis connection string
    /// </summary>
    /// <example>localhost:6379,password=your_password,ssl=False,abortConnect=False</example>
    /// <remarks>
    /// Format: hostname:port,password=password,ssl=True|False,abortConnect=True|False
    /// 
    /// Common options:
    /// - password: Authentication password
    /// - ssl: Enable/disable SSL/TLS encryption
    /// - abortConnect: Whether to stop connection attempts if initial attempt fails
    /// </remarks>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Validates that the connection string is provided
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when ConnectionString is null or empty</exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new ArgumentNullException(nameof(ConnectionString));
    }
}
