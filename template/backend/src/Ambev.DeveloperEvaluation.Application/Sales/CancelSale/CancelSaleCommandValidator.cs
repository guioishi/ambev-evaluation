using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .Length(26).WithMessage("Sale number must be 26 characters.")
            .Matches(@"^[a-zA-Z0-9-]+$").WithMessage("Sale number must contain only letters, numbers and hyphens.");
    }
}
