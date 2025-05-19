using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesQueryValidator : AbstractValidator<GetSalesQuery>
{
    public GetSalesQueryValidator()
    {
        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("Start date must be less than or equal to end date.");

        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .When(x => !x.FromDate.HasValue && !x.ToDate.HasValue)
            .WithMessage("At least one search criteria must be provided (dates or branch ID).");
    }
}
