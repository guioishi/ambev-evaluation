using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Product;

public class AddProductsToSaleCommandValidator : AbstractValidator<AddProductsToSaleCommand>
{
    public AddProductsToSaleCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one product is required");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID is required");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero");
            });
    }
}
