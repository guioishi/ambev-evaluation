using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler to cancel a sale
/// </summary>
public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, CancelSaleResultDto>
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleCommandHandler> _logger;
    private readonly ICacheService _cache;
    private const string CachePrefix = "sale:";

    public CancelSaleCommandHandler(
        ISaleRepository saleRepository, 
        IMapper mapper, 
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork,
        ILogger<CancelSaleCommandHandler> logger, ICacheService cache)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CancelSaleResultDto> Handle(
        CancelSaleCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing cancellation command for sale number {ProductId}", command.SaleNumber);

        var validator = new CancelSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);

        if (sale is null)
        {
            _logger.LogWarning("Sale not found: {ProductId}", command.SaleNumber);
            throw new SaleNotFoundException(command.SaleNumber);
        }

        if (sale.IsCancelled)
        {
            _logger.LogWarning("Attempted to cancel an already cancelled sale: {ProductId}", command.SaleNumber);
            throw new SaleAlreadyCancelledException(command.SaleNumber);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            sale.Cancel();

            await _saleRepository.UpdateAsync(sale, cancellationToken);
            _logger.LogDebug("Sale {ProductId} marked as cancelled in database", sale.SaleNumber);

            var cacheKey = $"{CachePrefix}{command.SaleNumber}";
            await _cache.RemoveAsync(cacheKey);

            var @event = new SaleCancelledEvent(
                sale.Id,
                DateTime.UtcNow,
                sale.Customer.CustomerId,
                "N/A");

            try
            {
                await _eventPublisher.PublishAsync(RedisChannels.SaleEvents, @event);
                await _eventPublisher.PublishAsync(RedisChannels.CacheInvalidation, @event);
                _logger.LogInformation("Sale cancelled events published for sale {ProductId}", sale.SaleNumber);
            }
            catch (Exception ex)
            {
                throw new SaleEventPublishingFailedException(command.SaleNumber, "SaleCancelled", ex);
            }

            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Sale {ProductId} cancelled successfully", sale.SaleNumber);

            return _mapper.Map<CancelSaleResultDto>(sale);
        }
        catch (Exception ex) when (ex is not DomainException)
        {
            _logger.LogError(ex, 
                "Error cancelling sale {ProductId}. Rolling back transaction", 
                command.SaleNumber);
        
            await _unitOfWork.RollbackAsync();
            throw new SaleTransactionFailedException(command.SaleNumber, ex);
        }
    }
}
