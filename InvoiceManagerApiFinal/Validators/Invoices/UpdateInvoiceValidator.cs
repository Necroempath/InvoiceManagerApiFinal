using FluentValidation;
using InvoiceManagerApi.DTOs.InvoiceDTOs;

namespace InvoiceManagerApi.Validators.Invoices;

public class UpdateInvoiceValidator : AbstractValidator<InvoiceUpdateRequest>
{
    public UpdateInvoiceValidator()
    {
        RuleFor(i => i.StartDate)
            .NotEmpty().WithMessage("Start Date is required")
            .LessThan(DateTime.UtcNow).WithMessage("Start Date must be less than current date.")
            .LessThan(i => i.EndDate).WithMessage("Start Date must be less than End Date.");

        RuleFor(i => i.EndDate)
            .NotEmpty().WithMessage("End Date is required")
            .LessThan(DateTime.UtcNow).WithMessage("End Date must be less than current date.")
            .GreaterThan(i => i.EndDate).WithMessage("End Date must be greater than Start Date.");
    }
}
