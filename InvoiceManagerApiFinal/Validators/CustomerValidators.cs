using FluentValidation;
using InvoiceManagerApiFinal.DTOs;

namespace InvoiceManagerApiFinal.Validators;

public class CreateCustomerValidator : AbstractValidator<CustomerCreateRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Customer Name is required")
            .MinimumLength(2).WithMessage("Customer Name length should be at least 2 characters");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Customer Email is required")
            .EmailAddress().WithMessage("Invalid Email");
    }
}
