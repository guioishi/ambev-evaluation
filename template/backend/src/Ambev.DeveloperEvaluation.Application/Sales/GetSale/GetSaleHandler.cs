using FluentValidation;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, GetSaleResultDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleQueryHandler> _logger;
    private readonly ICacheService _cache;
    private const string CachePrefix = "sale:";

    public GetSaleQueryHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<GetSaleQueryHandler> logger, ICacheService cache)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<GetSaleResultDto> Handle(
        GetSaleQuery command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving sale details for sale number {ProductId}", command.SaleNumber);
        var cacheKey = $"{CachePrefix}{command.SaleNumber}";

        var validator = new GetSaleQueryValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        try
        {
            var cached = await _cache.GetAsync<GetSaleResultDto>(cacheKey);
            if (cached is not null) return cached;

            var sale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);

            if (sale is null)
            {
                _logger.LogWarning("Sale not found with sale number {ProductId}", command.SaleNumber);
                throw new SaleNotFoundException(command.SaleNumber);
            }

            _logger.LogDebug(
                "Sale found - Id: {SaleId}, Customer: {@Customer}, Branch: {@Branch}, IsCancelled: {IsCancelled}",
                sale.Id,
                sale.Customer,
                sale.Branch,
                sale.IsCancelled);

            var result = _mapper.Map<GetSaleResultDto>(sale);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            _logger.LogInformation("Successfully retrieved sale details for sale number {ProductId}",
                command.SaleNumber);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving sale details for sale number {ProductId}",
                command.SaleNumber);

            throw new SaleOperationException(
                $"An error occurred while retrieving sale {command.SaleNumber}",
                ex);
        }
    }
}
