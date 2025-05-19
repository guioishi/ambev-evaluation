using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;
    private IDbContextTransaction? _transaction;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(DefaultContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        _logger.LogInformation("Database transaction started");
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();

                _logger.LogInformation("Transaction committed successfully");
            }
            else
            {
                _logger.LogWarning("Commit attempted but no active transaction found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transaction commit. Initiating rollback");
            await RollbackAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            _logger.LogInformation("Transaction rolled back successfully");
        }
        else
        {
            _logger.LogWarning("Rollback attempted but no active transaction found");
        }
    }
}
