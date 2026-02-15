using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

/// <summary>
/// DTO for Invoice update. Used for PUT requests.
/// </summary>
public class InvoiceUpdateRequest
{
    /// <summary>
    /// Updated start date of the service period
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Updated end date of the service period
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Updated invoice comment
    /// </summary>
    /// <example>Updated service period and pricing</example>
    public string? Comment { get; set; }
}
