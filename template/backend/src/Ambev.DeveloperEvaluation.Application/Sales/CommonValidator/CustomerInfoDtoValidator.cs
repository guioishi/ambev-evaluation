using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CommonValidator;

public class CustomerInfoDtoValidator : AbstractValidator<CustomerInfoDto>
{
    public CustomerInfoDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in valid international format.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Customer category is required.");
    }
}
