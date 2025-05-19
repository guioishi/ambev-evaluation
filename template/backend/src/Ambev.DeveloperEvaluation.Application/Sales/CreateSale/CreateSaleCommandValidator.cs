using Ambev.DeveloperEvaluation.Application.Sales.CommonValidator;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sale must contain at least one item.")
            .Must(items => items.Count > 0).WithMessage("Sale must contain at least one item.")
            .ForEach(item => item.SetValidator(new SaleProductDtoValidator()));

        RuleFor(x => x.Customer)
            .NotNull().WithMessage("Customer information is required.")
            .SetValidator(new CustomerInfoDtoValidator());
    }
}

public class SaleProductDtoValidator : AbstractValidator<SaleProductDto>
{
    public SaleProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}
