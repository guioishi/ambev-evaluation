using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Product;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResultDto>
{
    private readonly ISaleRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CreateSaleCommandHandler> _logger;

    public CreateSaleCommandHandler(ISaleRepository repository, IMapper mapper, IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork, ILogger<CreateSaleCommandHandler> logger, IProductRepository productRepository,
        ICacheService cacheService)
    {
        _repository = repository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<SaleResultDto> Handle(
        CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting sale creation process for branch {BranchId}", command.BranchId);

        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = _mapper.Map<CustomerInfo>(command.Customer);

        var sale = Sale.Create(
            customer,
            command.BranchId,
            command.BranchName);

        foreach (var item in command.Items)
        {
            var product = await GetProductWithCache(item.ProductId, cancellationToken);
            sale.AddProduct(item.ProductId, item.Quantity);

            _logger.LogDebug("Product {ProductId} added to sale {SaleId}", item.ProductId, sale.Id);

            sale.CalculateTotalPrice(product.Price, item.Quantity);
        }

        _logger.LogDebug("Sale created with {ItemCount} items", command.Items.Count);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _repository.CreateAsync(sale, cancellationToken);
            _logger.LogDebug("Sale {SaleId} persisted to database", sale.Id);

            try
            {
                var @event = new SaleCreatedEvent(sale.Id, DateTime.UtcNow);
                await _eventPublisher.PublishAsync(RedisChannels.SaleEvents, @event);
                _logger.LogInformation("Sale created event published for sale {SaleId}", sale.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new SaleEventPublishingFailedException(sale.SaleNumber, "SaleCreated", ex);
            }

            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Sale {SaleId} created successfully", sale.Id);

            return _mapper.Map<SaleResultDto>(sale);
        }
        catch (Exception ex) when (ex is not DomainException)
        {
            _logger.LogError(ex,
                "Error creating sale for branch {BranchId}. Rolling back transaction",
                command.BranchId);

            await _unitOfWork.RollbackAsync();
            throw new SaleCreationFailedException(command.BranchId, ex);
        }
    }

    private async Task<Domain.Entities.Product.Product> GetProductWithCache(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{productId}";

        var cachedProduct = await _cacheService.GetAsync<Domain.Entities.Product.Product>(cacheKey);
        if (cachedProduct != null)
        {
            return cachedProduct;
        }

        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new ProductException.ProductNotFoundException(productId);
        }

        await _cacheService.SetAsync(cacheKey, product, TimeSpan.FromMinutes(30));
        return product;
    }
}
