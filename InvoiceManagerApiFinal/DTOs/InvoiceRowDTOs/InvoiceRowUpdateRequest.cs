using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

/// <summary>
/// DTO for Invoice Row update. Used for PUT requests.
/// </summary>
public class InvoiceRowUpdateRequest
{
    /// <summary>
    /// Updated name or description of the provided service
    /// </summary>
    /// <example>Backend API development</example>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Updated quantity of the provided service
    /// </summary>
    /// <example>12</example>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Updated rate per single service unit
    /// </summary>
    /// <example>80.00</example>
    public decimal Rate { get; set; }
}
