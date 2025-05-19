namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

/// <summary>
/// Defines the contract for managing database transactions using the Unit of Work pattern.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction, saving all changes to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task CommitAsync();

    /// <summary>
    /// Rolls back the current transaction, discarding all pending changes.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RollbackAsync();
}
