using InvoiceManagerApi.Models;
using Microsoft.AspNetCore.Identity;

namespace InvoiceManagerApiFinal.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Address { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; }

    public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
}
