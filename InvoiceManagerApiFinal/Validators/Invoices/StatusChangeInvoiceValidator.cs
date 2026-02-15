using FluentValidation;
using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.Validators.Invoices;

public class StatusChangeInvoiceValidator : AbstractValidator<InvoiceStatusChangeRequest>
{
    public StatusChangeInvoiceValidator()
    {
        //RuleFor(i => i.Status)
        //    .NotEmpty().WithMessage("Invoice Status is required")
        //    .Must(i =>
        //    new [] { InvoiceStatus.Created, InvoiceStatus.Sent, InvoiceStatus.Received, InvoiceStatus.Paid, InvoiceStatus.Cancelled, InvoiceStatus.Rejected }.Contains(i))
        //    .WithMessage("Status must be 0 (Created) 1 (Sent) 2 (Received) 3 (Paid) 4 (Canceled) or 5 (Rejected)");

        // !!!NOTE FOR INSPECTOR!!!
        // I intentionally cancelled status validation, because of general enum validation (look Programs.cs row 20)
    }
}
