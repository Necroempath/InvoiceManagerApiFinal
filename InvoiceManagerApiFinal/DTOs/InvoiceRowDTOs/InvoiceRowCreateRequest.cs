namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

/// <summary>
/// DTO for Invoice Row creation. Used for POST requests.
/// </summary>
public class InvoiceRowCreateRequest
{
    /// <summary>
    /// Name or description of the provided service
    /// </summary>
    /// <example>Web development services</example>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the provided service
    /// </summary>
    /// <example>10</example>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Rate per single service unit
    /// </summary>
    /// <example>50.00</example>
    public decimal Rate { get; set; }

    /// <summary>
    /// Identifier of the related invoice
    /// </summary>
    /// <example>5</example>
    public int InvoiceId { get; set; }
}
