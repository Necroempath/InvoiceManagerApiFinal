namespace InvoiceManagerApi.DTOs.CustomerDTOs;

/// <summary>
/// DTO for returned Customer data. Uses for GET requests
/// </summary>
public class CustomerResponseDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public int InvoiceCount { get; set; }
    public decimal? InvoicesSum { get; set; }
    public string UserId { get; set; } = null!;
}
