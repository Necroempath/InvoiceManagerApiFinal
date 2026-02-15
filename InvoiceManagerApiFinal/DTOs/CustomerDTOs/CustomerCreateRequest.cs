namespace InvoiceManagerApi.DTOs.CustomerDTOs;

/// <summary>
/// DTO for Customer creation. Uses for POST requests
/// </summary>
public class CustomerCreateRequest
{
    /// <summary>
    /// Customer Name
    /// </summary>
    /// <example>John</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Customer Email
    /// </summary>
    /// <example>your@email.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Customer Address
    /// </summary>
    /// <example>Baku, Nizami St. 12</example>
    public string? Address { get; set; }

    /// <summary>
    /// Customer Phone Number
    /// </summary>
    /// <example>+9946661135</example>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Id of User Customer assigned to
    /// </summary>
    /// <example>1</example>
    public string UserId { get; set; } = null!;
}