using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

/// <summary>
/// DTO for invoice status modify. Uses for PATCH requests
/// </summary>
public class InvoiceStatusChangeRequest
{
    /// <summary>
    /// Invoice status
    /// </summary>
    /// <example>Sent</example>
    public InvoiceStatus Status { get; set; }
}
