using Ambev.DeveloperEvaluation.Domain.Exceptions.Product;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsQueryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductsQueryResult> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Retrieving products with pagination: PageNumber={PageNumber}, PageSize={PageSize}",
            query.PageNumber,
            query.PageSize);

        try
        {
            var totalItems = await _productRepository.GetTotalCountAsync(cancellationToken);

            var products = await _productRepository.GetPaginatedAsync(
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            var productsList = products.ToList();

            _logger.LogInformation(
                "Retrieved {Count} products (Page {PageNumber} of {TotalPages})",
                productsList.Count,
                query.PageNumber,
                (int)Math.Ceiling((double)totalItems / query.PageSize));

            return new GetProductsQueryResult
            {
                Products = _mapper.Map<List<ProductListItemDto>>(products),
                TotalItems = await _productRepository.GetTotalCountAsync(cancellationToken)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving products with pagination: PageNumber={PageNumber}, PageSize={PageSize}",
                query.PageNumber,
                query.PageSize);

            throw new ProductException.ProductOperationException(
                $"Error retrieving products for page {query.PageNumber} with size {query.PageSize}",
                ex);
        }
    }
}
