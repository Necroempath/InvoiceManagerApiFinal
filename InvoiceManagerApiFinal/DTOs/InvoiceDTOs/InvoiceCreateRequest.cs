namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

/// <summary>
/// DTO for Invoice creation. Uses for GET requests
/// </summary>
public class InvoiceCreateRequest
{
    /// <summary>
    /// Start date of service
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// End date of service
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Invoice comment
    /// </summary>
    /// <example>Your comment</example>
    public string? Comment { get; set; }

    /// <summary>
    /// Customer ID
    /// </summary>
    /// <example>1</example>
    public int CustomerId { get; set; }
}
