using FluentValidation;
using InvoiceManagerApiFinal.DTOs;

namespace InvoiceManagerApiFinal.Validators;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Password is required")
            .CustomPassword().WithMessage("Password must contain [a-z] [A-Z] digits and be at least 6 characters long");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password is required")
            .CustomPassword().WithMessage("Password must contain [a-z] [A-Z] digits and be at least 6 characters long");
    }
}

public class ChangeProfileValidator : AbstractValidator<ChangeProfileRequest>
{
    public ChangeProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Password must and be at least 2 characters long");
    }
}