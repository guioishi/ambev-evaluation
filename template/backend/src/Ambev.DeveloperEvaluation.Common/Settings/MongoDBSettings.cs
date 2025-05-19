namespace Ambev.DeveloperEvaluation.Common.Settings;

/// <summary>
/// Configuration settings for MongoDB connections and collections
/// </summary>
public class MongoDBSettings
{
    /// <summary>
    /// MongoDB connection string
    /// </summary>
    /// <example>mongodb://username:password@localhost:27017</example>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Name of the MongoDB database to use
    /// </summary>
    /// <example>DeveloperStore</example>
    public string DatabaseName { get; set; }

    /// <summary>
    /// Name of the collection storing sales documents
    /// </summary>
    /// <example>Sales</example>
    public string SalesCollectionName { get; set; }

    /// <summary>
    /// Name of the collection storing event documents
    /// </summary>
    /// <example>Events</example>
    public string EventsCollectionName { get; set; }

    /// <summary>
    /// Validates that all required settings are provided
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the required settings is null or empty:
    /// - ConnectionString
    /// - DatabaseName
    /// - SalesCollectionName
    /// - EventsCollectionName
    /// </exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new ArgumentNullException(nameof(ConnectionString));
        if (string.IsNullOrEmpty(DatabaseName))
            throw new ArgumentNullException(nameof(DatabaseName));
        if (string.IsNullOrEmpty(SalesCollectionName))
            throw new ArgumentNullException(nameof(SalesCollectionName));
        if (string.IsNullOrEmpty(EventsCollectionName))
            throw new ArgumentNullException(nameof(EventsCollectionName));
    }
}
