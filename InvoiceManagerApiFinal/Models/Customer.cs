using InvoiceManagerApiFinal.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InvoiceManagerApi.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string UserId { get; set; }
    public User User { get; set; } = null!;
    public IEnumerable<Invoice> Invoices { get; set; } = new List<Invoice>();
}   