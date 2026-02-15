using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

/// <summary>
/// DTO for Invoice Row response. Used for GET requests.
/// </summary>
public class InvoiceRowResponseDto
{
    /// <summary>
    /// Unique identifier of the invoice row
    /// </summary>
    /// <example>15</example>
    public int Id { get; set; }

    /// <summary>
    /// Name or description of the provided service
    /// </summary>
    /// <example>Web development services</example>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the provided service
    /// </summary>
    /// <example>8</example>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Rate per single service unit
    /// </summary>
    /// <example>75.00</example>
    public decimal Rate { get; set; }

    /// <summary>
    /// Total amount for this invoice row (Quantity × Rate)
    /// </summary>
    /// <example>600.00</example>
    public decimal Sum { get; set; }

    /// <summary>
    /// Identifier of the related invoice
    /// </summary>
    /// <example>5</example>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Current status of the related invoice
    /// </summary>
    /// <example>Draft</example>
    public string InvoiceStatus { get; set; } = string.Empty;
}

