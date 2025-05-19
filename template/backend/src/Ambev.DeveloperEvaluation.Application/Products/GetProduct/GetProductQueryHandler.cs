using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Product;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQueryCommand, GetProductResultDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductQueryHandler> _logger;

    public GetProductQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResultDto> Handle(
        GetProductQueryCommand queryCommand,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving product with ID: {ProductId}", queryCommand.ProductId);

        try
        {
            var product = await _productRepository.GetByIdAsync(queryCommand.ProductId, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId}", queryCommand.ProductId);
                throw new ProductException.ProductNotFoundException(queryCommand.ProductId);
            }

            var result = _mapper.Map<GetProductResultDto>(product);

            _logger.LogInformation("Successfully retrieved product with ID: {ProductId}", product.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID: {ProductId}", queryCommand.ProductId);
            throw new ProductException.ProductOperationException($"Error retrieving product with ID {queryCommand.ProductId}",
                ex);
        }
    }
}
