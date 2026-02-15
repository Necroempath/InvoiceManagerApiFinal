using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public IEnumerable<InvoiceRow> Rows { get; set; } = new List<InvoiceRow>();
    public decimal TotalSum { get; set; }
    public string? Comment { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Created;
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
