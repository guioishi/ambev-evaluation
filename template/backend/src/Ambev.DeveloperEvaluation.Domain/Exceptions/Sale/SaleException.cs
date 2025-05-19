namespace Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;

/// <summary>
/// Exception thrown when a sale cannot be found in the system
/// </summary>
public class SaleNotFoundException : DomainException
{
    /// <summary>
    /// The sale number that was not found
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Creates a new instance of SaleNotFoundException
    /// </summary>
    /// <param name="saleNumber">The sale number that was not found</param>
    public SaleNotFoundException(string saleNumber) 
        : base($"Sale with number '{saleNumber}' not found")
    {
        SaleNumber = saleNumber;
    }
}

/// <summary>
/// Exception thrown when attempting to cancel a sale that is already cancelled
/// </summary>
public class SaleAlreadyCancelledException : DomainException
{
    /// <summary>
    /// The sale number that is already cancelled
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Creates a new instance of SaleAlreadyCancelledException
    /// </summary>
    /// <param name="saleNumber">The sale number that is already cancelled</param>
    public SaleAlreadyCancelledException(string saleNumber) 
        : base($"Sale '{saleNumber}' is already cancelled")
    {
        SaleNumber = saleNumber;
    }
}

/// <summary>
/// Exception thrown when the cancellation process of a sale fails
/// </summary>
public class SaleCancellationFailedException : DomainException
{
    /// <summary>
    /// The sale number that failed to cancel
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Creates a new instance of SaleCancellationFailedException
    /// </summary>
    /// <param name="saleNumber">The sale number that failed to cancel</param>
    /// <param name="innerException">The underlying cause of the failure</param>
    public SaleCancellationFailedException(string saleNumber, Exception innerException) 
        : base($"Failed to cancel sale '{saleNumber}'", innerException)
    {
        SaleNumber = saleNumber;
    }
}

/// <summary>
/// Exception thrown when publishing a sale event fails
/// </summary>
public class SaleEventPublishingFailedException : DomainException
{
    /// <summary>
    /// The sale number for which event publishing failed
    /// </summary>
    public string SaleNumber { get; }
    
    /// <summary>
    /// The type of event that failed to publish
    /// </summary>
    public string EventType { get; }

    /// <summary>
    /// Creates a new instance of SaleEventPublishingFailedException
    /// </summary>
    /// <param name="saleNumber">The sale number for which event publishing failed</param>
    /// <param name="eventType">The type of event that failed to publish</param>
    /// <param name="innerException">The underlying cause of the failure</param>
    public SaleEventPublishingFailedException(string saleNumber, string eventType, Exception innerException) 
        : base($"Failed to publish {eventType} event for sale '{saleNumber}'", innerException)
    {
        SaleNumber = saleNumber;
        EventType = eventType;
    }
}

/// <summary>
/// Exception thrown when a transaction involving a sale operation fails
/// </summary>
public class SaleTransactionFailedException : DomainException
{
    /// <summary>
    /// The sale number for which the transaction failed
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Creates a new instance of SaleTransactionFailedException
    /// </summary>
    /// <param name="saleNumber">The sale number for which the transaction failed</param>
    /// <param name="innerException">The underlying cause of the failure</param>
    public SaleTransactionFailedException(string saleNumber, Exception innerException) 
        : base($"Transaction failed while processing sale '{saleNumber}'", innerException)
    {
        SaleNumber = saleNumber;
    }
}

/// <summary>
/// Base exception for general sale operations errors
/// </summary>
public class SaleOperationException : DomainException
{
    /// <summary>
    /// Creates a new instance of SaleOperationException with a message
    /// </summary>
    /// <param name="message">Description of the error</param>
    public SaleOperationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of SaleOperationException with a message and inner exception
    /// </summary>
    /// <param name="message">Description of the error</param>
    /// <param name="innerException">The underlying cause of the failure</param>
    public SaleOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when creating a new sale fails
/// </summary>
public class SaleCreationFailedException : DomainException
{
    /// <summary>
    /// The ID of the branch where the sale creation was attempted
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Creates a new instance of SaleCreationFailedException
    /// </summary>
    /// <param name="branchId">The ID of the branch where the sale creation was attempted</param>
    /// <param name="innerException">The underlying cause of the failure</param>
    public SaleCreationFailedException(Guid branchId, Exception innerException)
        : base($"Failed to create sale for branch '{branchId}'", innerException)
    {
        BranchId = branchId;
    }
}
