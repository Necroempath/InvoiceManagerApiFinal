using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

/// <summary>
/// DTO for Invoice response. Used for GET requests.
/// </summary>
public class InvoiceResponseDto
{
    /// <summary>
    /// Unique identifier of the invoice
    /// </summary>
    /// <example>10</example>
    public int Id { get; set; }

    /// <summary>
    /// Start date of the service period
    /// </summary>
    /// <example>2026-02-04T12:54:27.393Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// End date of the service period
    /// </summary>
    /// <example>2026-05-04T12:54:27.393Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Current invoice status
    /// </summary>
    /// <example>Paid</example>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Optional invoice comment
    /// </summary>
    /// <example>Monthly service payment</example>
    public string? Comment { get; set; }

    /// <summary>
    /// Total amount of the invoice
    /// </summary>
    /// <example>1250.75</example>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// Number of invoice rows
    /// </summary>
    /// <example>3</example>
    public int RowsCount { get; set; }

    /// <summary>
    /// Identifier of the related customer
    /// </summary>
    /// <example>1</example>
    public int CustomerId { get; set; }

    /// <summary>
    /// Name of the customer
    /// </summary>
    /// <example>John Doe</example>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the customer
    /// </summary>
    /// <example>john.doe@example.com</example>
    public string CustomerEmail { get; set; } = string.Empty;
}