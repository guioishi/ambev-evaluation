using Ambev.DeveloperEvaluation.Domain.Interfaces;

namespace Ambev.DeveloperEvaluation.Functional.Services;

/// <summary>
/// Transactions are not supported by the in-memory store.
/// See https://go.microsoft.com/fwlink/?LinkId=800142
/// This exception can be suppressed or logged by passing event ID 'InMemoryEventId.TransactionIgnoredWarning'
/// to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.
/// </summary>
public class MockUnitOfWork : IUnitOfWork
{
    public Task BeginTransactionAsync() => Task.CompletedTask;
    public Task CommitAsync() => Task.CompletedTask;
    public Task RollbackAsync() => Task.CompletedTask;
}
