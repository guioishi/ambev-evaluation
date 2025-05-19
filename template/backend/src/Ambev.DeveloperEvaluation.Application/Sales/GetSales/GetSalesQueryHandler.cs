using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, List<SaleResultDto>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSalesQueryHandler> _logger;

    public GetSalesQueryHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<GetSalesQueryHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<SaleResultDto>> Handle(
        GetSalesQuery command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Retrieving sales with filters: FromDate={FromDate}, ToDate={ToDate}, BranchId={BranchId}",
            command.FromDate,
            command.ToDate,
            command.BranchId);

        var validator = new GetSalesQueryValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // fails if we don't convert to UTC (postgres stores in UTC)
        var fromDateUtc = command.FromDate?.ToUniversalTime();
        var toDateUtc = command.ToDate?.ToUniversalTime();

        try
        {
            var sales = await _saleRepository.GetByFilterAsync(
                fromDateUtc,
                toDateUtc,
                command.BranchId,
                cancellationToken);

            var salesList = sales.ToList();

            _logger.LogInformation(
                "Retrieved {Count} sales matching the specified criteria",
                salesList.Count);

            _logger.LogDebug(
                "Sales retrieved with IDs: {SaleIds}",
                string.Join(", ", salesList.Select(s => s.Id)));

            var result = _mapper.Map<List<SaleResultDto>>(salesList);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving sales with filters: FromDate={FromDate}, ToDate={ToDate}, BranchId={BranchId}",
                fromDateUtc,
                toDateUtc,
                command.BranchId);

            throw new SaleOperationException(
                $"An error occurred while retrieving sale for dates {fromDateUtc} and {toDateUtc}",
                ex);
        }
    }
}
