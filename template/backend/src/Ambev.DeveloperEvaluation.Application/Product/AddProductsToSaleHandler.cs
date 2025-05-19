using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Product;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Product;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Product;

public class AddProductsToSaleHandler : IRequestHandler<AddProductsToSaleCommand, SaleResultDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddProductsToSaleHandler> _logger;
    private readonly ICacheService _cacheService;

    public AddProductsToSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AddProductsToSaleHandler> logger,
        ICacheService cacheService)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<SaleResultDto> Handle(
        AddProductsToSaleCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding products to sale {SaleId}", command.SaleId);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
        {
            throw new SaleNotFoundException(command.SaleId.ToString());
        }

        foreach (var item in command.Items)
        {
            // 2.1 Buscar o produto (com cache)
            var product = await GetProductWithCache(item.ProductId, cancellationToken);

            // 2.2 Adicionar produto à venda
            sale.AddProduct(item.ProductId, item.Quantity);

            // 3. Atualizar o valor total da venda
            sale.CalculateTotalPrice(product.Price, item.Quantity);
        }

        _logger.LogDebug("Added {ItemCount} products to sale {SaleId}", command.Items.Count, sale.Id);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 4. Persistir alterações
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            _logger.LogDebug("Sale {SaleId} updated in database", sale.Id);

            // 5. Publicar evento
            try
            {
                var @event = new ProductsAddedToSaleEvent(sale.Id, DateTime.UtcNow, command.Items.Count);
                await _eventPublisher.PublishAsync(RedisChannels.ProductEvents, @event);
                _logger.LogInformation("Products added event published for sale {SaleId}", sale.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new SaleEventPublishingFailedException(sale.SaleNumber, "ProductsAdded", ex);
            }

            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Products added successfully to sale {SaleId}", sale.Id);

            return _mapper.Map<SaleResultDto>(sale);
        }
        catch (Exception ex) when (ex is not DomainException)
        {
            _logger.LogError(ex,
                "Error adding products to sale {SaleId}. Rolling back transaction",
                command.SaleId);

            await _unitOfWork.RollbackAsync();
            throw new SaleTransactionFailedException(command.SaleId.ToString(), ex);
        }
    }

    private async Task<Domain.Entities.Product.Product> GetProductWithCache(Guid productId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{productId}";

        // Tentar obter do cache primeiro
        var cachedProduct = await _cacheService.GetAsync<Domain.Entities.Product.Product>(cacheKey);
        if (cachedProduct != null)
        {
            return cachedProduct;
        }

        // Se não encontrou no cache, buscar no repositório
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new ProductException.ProductNotFoundException(productId);
        }

        // Armazenar no cache
        await _cacheService.SetAsync(cacheKey, product, TimeSpan.FromMinutes(30));

        return product;
    }
}
